using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;

public enum RoundState
{
	PLAYERDEATH,
	DUNGEONDEATH,
	NODEATH
}
public partial class FightLogic : Node
{
	[Signal] 
    public delegate void FightEndedEventHandler();


	List<Card> PlayerDeck = new List<Card>();
	List<Card> DungeonDeck = new List<Card>();

	[Export] Label RoundText;
	[Export] HBoxContainer IconContainer;
	[Export] Control PlayerCardControl;
	[Export] Control EnemyCardControl;
	[Export] Control NextPlayerCardControl;
	[Export] Control NextEnemyCardControl;

	[Export] AnimationPlayer PlayerAnimPlayer;
	[Export] AnimationPlayer DungeonAnimPlayer;
	[Export] AnimationPlayer WorldAnimPlayer;
	[Export] AnimationPlayer NextCardsAnimPlayer;

	[Export] Button StepBattleButton;

	[Export] TextureRect healthPotionIcon;
	[Export] TextureRect glassShieldIcon;
	[Export] TextureRect elementalBuffIcon;
	Card winningCard;

	public Card PlayerCard;
	public Card DungeonCard;
	List<IItem> ItemList = new List<IItem>();
	private int Round = 0;
	public Player player = Global.gameManager.saverLoader.currSaveFile.player;
	public DungeonRewardTypes reward;
	public DungeonTypes type;
	private int PlayerDamage = 0;
	private int DungeonDamage = 0;
	private bool PlayerDied;
	private bool IsFirstRound = true;
	private bool IsAnimationFinished = false;
	private bool IsEnded = false;

	public override void _Ready()
	{
		foreach (Card card in player.Deck) PlayerDeck.Add(card is BossCard ? new BossCard(card as BossCard) : new Card(card));
		// foreach (Card card in Global.gameManager.saverLoader.currSaveFile.player.Deck) PlayerDeck.Add(new Card(card));
		Dungeon dungeon = Utility.ReturnDungeonFromList(Global.gameManager.saverLoader.currSaveFile.WorldDungeons, Global.gameManager.saverLoader.currSaveFile.currDungeonName);
		foreach (Card card in dungeon.DungeonDeck) DungeonDeck.Add(card is BossCard ? new BossCard(card as BossCard) : new Card(card));
		reward = dungeon.DungeonReward;
		type = dungeon.DungeonType;
		LoadItemButtons(player.ItemList);
		DungeonAnimPlayer.AnimationFinished += PlayerAttack;
		PlayerAnimPlayer.AnimationFinished += LoadBattleItems;

		WorldAnimPlayer.Play("EntryAnimation");
        Global.gameManager.audioController.PlayMusicAndEnvSounds(Global.gameManager.audioController.audioBank.FightMusic, Global.gameManager.audioController.audioBank.CaveAmbiance);


	}

	private void ReasignPlayerCard()
	{
		if (PlayerDeck.Count > 0)
		{
			Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.hoverSounds.PickRandom());

			PlayerCard = PlayerDeck[0];
			TryReassignNextCard(PlayerDeck, NextPlayerCardControl);
			_SetItemIconsInvisible();
			PlayerDeck.RemoveAt(0);
			return;
		}
		PlayerCard = null;
	}

	private void ReasignDungeonCard()
	{
		if (DungeonDeck.Count > 0)
		{
			Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.hoverSounds.PickRandom());

			DungeonCard = DungeonDeck[0];
			TryReassignNextCard(DungeonDeck, NextEnemyCardControl);
			DungeonDeck.RemoveAt(0);
			return;
		}
		DungeonCard = null;
	}

	private void TryReassignNextCard(List<Card> from, Control onto)
    {
		foreach (Control child in onto.GetChildren())
        {
            child.QueueFree();
        }
		if (from.Count > 1)
		{
			Card nextCard = from[1];
			Utility.AddUiCardUnderContainer(nextCard, onto);
			NextCardsAnimPlayer.Play("NextCardsSpawn");
			return;
		}

		TextureRect newTexRect = new TextureRect();
		newTexRect.Texture = GD.Load<Texture2D>("uid://ddjir7c4i3gte");
		newTexRect.ExpandMode = TextureRect.ExpandModeEnum.FitWidth;
		newTexRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
		newTexRect.CustomMinimumSize = new Vector2(100.0f,100.0f);

		onto.AddChild(newTexRect);

		NextCardsAnimPlayer.Play("NextCardsSpawn");

    }


	private void ClearBuffs()
	{
		PlayerCard.buffHandler.ClearBuffs(Round);
		DungeonCard.buffHandler.ClearBuffs(Round);
	}

	public void LoadItemButtons(List<IItem> itemList)
	{
		foreach (ItemButton2VigyeElAMento child in IconContainer.GetChildren())
		{
			IconContainer.RemoveChild(child);
		}
		PackedScene iconButtonScene = GD.Load<PackedScene>("uid://cy0pttedf86kl");

		foreach (IItem item in itemList)
		{
			ItemButton2VigyeElAMento btn = iconButtonScene.Instantiate<ItemButton2VigyeElAMento>();
			btn.itemType = item.Type;
			if (player.ReturnItemAmount(item) == 0) btn.Disable();
			btn.Send_Item += OnAddToItemListPressed;
			btn.useButton.Disabled = true;
			IconContainer.AddChild(btn);
		}
	}

	public void LoadBattleItems(StringName name)
	{
		LoadDungeonCard();
		LoadPlayerCard();

		if (DungeonCard == null || PlayerCard == null)
			EndFight(PlayerCard != null ? $"Player Won {PlayerCard.Name}, {PlayerCard.Health}" : $"Enemy Won {DungeonCard.Name}, {DungeonCard.Health}");
	}

	private void LoadDungeonCard()
	{
		if (DungeonCard != null)
		{
			Utility.AddUiCardUnderContainer(DungeonCard, EnemyCardControl);
		}
	}

	private void LoadPlayerCard()
	{
		if (PlayerCard != null)
		{
			Utility.AddUiCardUnderContainer(PlayerCard, PlayerCardControl);
		}
	}

	public void DisplayAttack(bool isPlayerAttacked)
	{
		PackedScene damageLabel = GD.Load<PackedScene>("uid://bq8tgihxie003");
		DamageLabel labelInstance = damageLabel.Instantiate<DamageLabel>();
		labelInstance.Text = isPlayerAttacked ? DungeonDamage.ToString() : PlayerDamage.ToString();
		labelInstance.ZIndex = 100;
		if (isPlayerAttacked)
			labelInstance.GlobalPosition = PlayerCardControl.GlobalPosition;
		else
			labelInstance.GlobalPosition = EnemyCardControl.GlobalPosition;
		labelInstance.GlobalPosition += new Vector2(0, -200);
		GetParent().GetNode<CanvasLayer>("CanvasLayer").AddChild(labelInstance);
	}

	private void PlayerAttack(StringName animName)
	{
		if (!PlayerDied)
			PlayerAnimPlayer.Play("PlayerAttack");
		else
		{
			LoadBattleItems("");
		}
	}

	public void PlayAttackSequence()
	{
		IsAnimationFinished = false;
		PlayerAnimPlayer.Seek(0, true);
		PlayerAnimPlayer.Stop();
		DungeonAnimPlayer.Seek(0, true);
		DungeonAnimPlayer.Stop();
		DungeonAnimPlayer.Play("DungeonAttack");

	}

	private void ApplyItems(List<IItem> itemList)
	{
		foreach (IItem item in itemList)
		{
			item.ApplyPlayerBuff(PlayerCard, Round);
			item.ApplyDungeonBuff(DungeonCard, Round);
			player.RemoveFromItemList(item);
		}
	}

	private void SimulateRoundOne()
	{
		WorldAnimPlayer.Play("CardSpawnFirst");
		Round++;
		RoundText.Text = $"Kör: {Round}";
		ReasignPlayerCard();
		ReasignDungeonCard();
		LoadBattleItems("");
		foreach (Node child in IconContainer.GetChildren())
        {
			ItemButton2VigyeElAMento btn = child as ItemButton2VigyeElAMento;
            btn.useButton.Disabled = false;
        }
		
	}

	public RoundState SimulateRound(Card DungeonCard, Card PlayerCard)
	{
		DungeonDamage = DungeonCard.Attack(PlayerCard, true);

		if (PlayerCard.Health == 0) return RoundState.PLAYERDEATH;

		PlayerDamage = PlayerCard.Attack(DungeonCard, false);

		if (DungeonCard.Health == 0) return RoundState.DUNGEONDEATH;

		return RoundState.NODEATH;
	}

	private void OnButtonPressed()
	{
		Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());

		if (IsFirstRound)
		{
			SimulateRoundOne();
			IsFirstRound = false;
			
			return;
		}
		if (DungeonCard != null && PlayerCard != null)
		{
			ApplyItems(ItemList);
			LoadBattleItems("");
			Round++;
			RoundText.Text = $"Round: {Round}";

			ClearBuffs();
			RoundState currRound = SimulateRound(DungeonCard, PlayerCard);
			PlayerDied = currRound == RoundState.PLAYERDEATH;
			ItemList.Clear();

			if (currRound == RoundState.PLAYERDEATH)
			{
				
				ReasignPlayerCard();
			}
			else if (currRound == RoundState.DUNGEONDEATH)
			{
				ReasignDungeonCard();
			}
			PlayAttackSequence();
			
			foreach (ItemButton2VigyeElAMento child in IconContainer.GetChildren())
			{
				if (player.ReturnItemAmount(Items.CreateItemFromType(child.itemType)) <= 0) child.Disable();
			}
		}
		else if (!IsEnded)
		{
			EndFight(PlayerCard != null ? $"Player Won {PlayerCard.Name}, {PlayerCard.Health}" : $"Enemy Won {DungeonCard.Name}, {DungeonCard.Health}");
		}
	}

	private void EndFight(string output)
	{

		IsEnded = true;
		PlayerAnimPlayer.Seek(0, true);
		PlayerAnimPlayer.Stop();
		DungeonAnimPlayer.Seek(0, true);
		DungeonAnimPlayer.Stop();

		if (PlayerCard != null)
		{
			Card card = player.ReturnCardFromCollection(PlayerCard.Name);
			if (card != null)
			{
				ApplyReward(card);
			}
		}

		EmitSignal(SignalName.FightEnded);
	}

	private void ApplyReward(Card card)
	{
		GD.Print(reward.ToString());
		if (reward == DungeonRewardTypes.health)
		{
			card.ModifyHealth(2);
		}
		else if (reward == DungeonRewardTypes.attack)
		{
			card.ModifyBaseDamage(1);
		}
		else
		{
			GD.Print("buh");
			List<string> PlayerCollectionNames = Utility.WorldObjectListToNameList(player.Collection);
			foreach (Card worldCard in Global.gameManager.saverLoader.currSaveFile.WorldCards)
			{
				GD.Print(worldCard);
				if (!PlayerCollectionNames.Contains(worldCard.Name))
				{
					winningCard = worldCard;
					player.TryAddCardToCollection(worldCard);
					break;
				}
			}
		}

		if (type == DungeonTypes.simple)
		{
			player.Xp += 5;
			player.Money += 5;
		}
		else if (type == DungeonTypes.small)
		{
			player.Xp += 10;
			player.Money += 10;
		}
		else
		{
			player.Xp += 20;
			player.Money += 20;
		}
	}

	public Card GiveWinningCard()
    {
        return winningCard;
    }

	public void OnAddToItemListPressed(int item)
	{
		IItem createdItem = Items.CreateItemFromType((ItemType)item);
		if (!Utility.ItemListToNameList(ItemList).Contains(createdItem.Name)) ItemList.Add(createdItem);

		switch ((ItemType)item)
        {
            case ItemType.HealthPotion:
        		Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.healthPotionSounds.PickRandom());
				healthPotionIcon.Visible = true;
			break;
			
            case ItemType.GlassShield:
        		Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.glassShieldSounds.PickRandom());
				glassShieldIcon.Visible = true;
			break;
			
            case ItemType.ElementalBuff:
        		Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.elementalBuffSounds.PickRandom());
				elementalBuffIcon.Visible = true;
			break;
        }
	}

	public void OnRemoveFromItemListPressed(int item)
	{
		IItem createdItem = Items.CreateItemFromType((ItemType)item);
		foreach (IItem curritem in ItemList)
			if (curritem.Name == createdItem.Name)
			{
				ItemList.Remove(curritem);
				return;
			}
	}


	public void _AnimationPlayAttackSFX()
    {
		Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.attackSounds.PickRandom());
    }

	public void _AnimationPlayDeathSFX()
    {
		Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.deathSounds.PickRandom());
    }

	public void _SetItemIconsInvisible()
    {
        healthPotionIcon.Visible = false;
        glassShieldIcon.Visible = false;
        elementalBuffIcon.Visible = false;
    }
}
