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
	List<Card> PlayerDeck = new List<Card>();
	// List<Card> DungeonDeck = new List<Card>();
	// List<IItem> ItemList = new List<IItem>();

	[Export] RichTextLabel RoundText;
	[Export] HBoxContainer IconContainer;
	[Export] Control PlayerCardControl;
	[Export] Control EnemyCardControl;
	[Export] AnimationPlayer PlayerAnimPlayer;
	[Export] AnimationPlayer DungeonAnimPlayer;
	[Export] Button StepBattleButton;

	// TEST
	// List<Card> PlayerDeck = new List<Card> { new Card("Corky", 2, 4, CardElements.EARTH), new Card("alma", 2, 7, CardElements.WIND) };
	Card PlayerCard;
	Card DungeonCard;
	List<Card> DungeonDeck = new List<Card> { new Card("Sadan", 2, 4, CardElements.WIND) };
	List<IItem> ItemList = new List<IItem>();
	private int Round = 0;
	Player player = Global.gameManager.saverLoader.currSaveFile.player;
	DungeonRewardTypes Reward;
	private int PlayerDamage = 0;
	private int DungeonDamage = 0;
	private bool PlayerDied;
	private bool IsFirstRound = true;
	private bool IsAnimationFinished = false;
	private bool IsEnded = false;

	public override void _Ready()
	{
		foreach (Card card in player.Deck) PlayerDeck.Add(new Card(card));
		// foreach (Card card in Global.gameManager.saverLoader.currSaveFile.player.Deck) PlayerDeck.Add(new Card(card));
		Dungeon dungeon = Utility.ReturnDungeonFromList(Global.gameManager.saverLoader.currSaveFile.WorldDungeons, Global.gameManager.saverLoader.currSaveFile.currDungeonName);
		GD.Print(dungeon == null);
		// DungeonDeck = dungeon.DungeonDeck;
		Reward = dungeon.DungeonReward;
		LoadItemButtons(player.ItemList);
		DungeonAnimPlayer.AnimationFinished += PlayerAttack;
		PlayerAnimPlayer.AnimationFinished += LoadBattleItems;
	}

	private void ReasignPlayerCard()
	{
		if (PlayerDeck.Count > 0)
		{
			PlayerCard = PlayerDeck[0];
			PlayerDeck.RemoveAt(0);
			return;
		}
		PlayerCard = null;
	}

	private void ReasignDungeonCard()
	{
		if (DungeonDeck.Count > 0)
		{
			DungeonCard = DungeonDeck[0];
			DungeonDeck.RemoveAt(0);
			return;
		}
		DungeonCard = null;
	}
	private void ClearBuffs()
	{
		PlayerCard.buffHandler.ClearBuffs(Round);
		DungeonCard.buffHandler.ClearBuffs(Round);
	}

	public void LoadItemButtons(List<IItem> itemList)
	{
		PackedScene iconButtonScene = GD.Load<PackedScene>("uid://duqvkh3tdlkve");

		foreach (IItem item in itemList)
		{
			ItemButton btn = iconButtonScene.Instantiate<ItemButton>();
			btn.itemType = item.Type;
			btn.SetToggledOn();
			btn.Send_Item_Added += OnAddToItemListPressed;
			btn.Send_Item_Removed += OnRemoveFromItemListPressed;
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
			GD.Print(Round + " kazamata: " + DungeonCard.Name);
		}
    }

	private void LoadPlayerCard()
    {
        if (PlayerCard != null)
		{
			Utility.AddUiCardUnderContainer(PlayerCard, PlayerCardControl);
			GD.Print(Round + " jatekos: " + PlayerCard.Name);
		}
    }

	public void DisplayAttack(bool isPlayerAttacked)
	{
		PackedScene damageLabel = GD.Load<PackedScene>("uid://bq8tgihxie003");
		DamageLabel labelInstance = damageLabel.Instantiate<DamageLabel>();
		labelInstance.Text = isPlayerAttacked ? PlayerDamage.ToString() : DungeonDamage.ToString();
		labelInstance.ZIndex = 100;
		if (isPlayerAttacked)
			labelInstance.GlobalPosition = EnemyCardControl.GlobalPosition;
		else
			labelInstance.GlobalPosition = PlayerCardControl.GlobalPosition;
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
		Round++;
		RoundText.Text = $"Round: {Round}";
		ReasignPlayerCard();
		ReasignDungeonCard();
		LoadBattleItems("");
	}

	public RoundState SimulateRound(Card DungeonCard, Card PlayerCard, List<IItem> itemList)
	{
		ApplyItems(itemList);

		DungeonDamage = DungeonCard.Attack(PlayerCard);
		GD.Print(Round + " Dungeon Attack: " + DungeonDamage);

		if (PlayerCard.Health == 0) return RoundState.PLAYERDEATH;

		PlayerDamage = PlayerCard.Attack(DungeonCard);
		GD.Print(Round + " Player Attack: " + PlayerDamage);

		if (DungeonCard.Health == 0) return RoundState.DUNGEONDEATH;

		return RoundState.NODEATH;
	}

	private void OnButtonPressed()
	{
		if (IsFirstRound)
		{
			SimulateRoundOne();
			IsFirstRound = false;
			return;
		}
		if (DungeonCard != null && PlayerCard != null)
		{
			LoadBattleItems("");
			Round++;
			RoundText.Text = $"Round: {Round}";

			ClearBuffs();
			RoundState currRound = SimulateRound(DungeonCard, PlayerCard, ItemList);
			PlayerDied = currRound == RoundState.PLAYERDEATH;
			ItemList.Clear();

			if (currRound == RoundState.PLAYERDEATH)
			{
				GD.Print($"{PlayerCard.Name} player card died");
				ReasignPlayerCard();
			}
			else if (currRound == RoundState.DUNGEONDEATH)
			{
				GD.Print($"{DungeonCard.Name} enemy card died");
				ReasignDungeonCard();
			}
			PlayAttackSequence();
        }
        else if (!IsEnded)
        {
            EndFight(PlayerDeck != null ? $"Player Won {PlayerCard.Name}, {PlayerCard.Health}" : $"Enemy Won {DungeonCard.Name}, {DungeonCard.Health}");
        }
	}

	private void OnItemPressed()
	{
		GD.Print("BRUH");
		GD.Print("pront");
	}

	private void EndFight(string output)
	{
		// StepBattleButton.Disabled = true;
		IsEnded = true;
		PlayerAnimPlayer.Seek(0, true);
		PlayerAnimPlayer.Stop();
		DungeonAnimPlayer.Seek(0, true);
		DungeonAnimPlayer.Stop();
		GD.Print(output + " " + Reward.ToString());
		if (PlayerCard != null)
		{
			GD.Print("ALMA2");
			Card card = player.ReturnCardFromCollection(PlayerCard.Name);
			if (card != null)
			{
				GD.Print("ALMA");
				ApplyReward(card);
			}
		}
	}

	private void ApplyReward(Card card)
	{
		if (Reward == DungeonRewardTypes.health)
		{
			card.ModifyHealth(2);
		}
		else if (Reward == DungeonRewardTypes.attack)
		{
			card.ModifyBaseDamage(1);
		}
		else
		{
			List<string> PlayerCollectionNames = Utility.WorldObjectListToNameList(player.Collection);
			foreach (Card worldCard in Global.gameManager.saverLoader.currSaveFile.WorldCards)
			{
				if (!PlayerCollectionNames.Contains(worldCard.Name))
				{
					player.TryAddCardToCollection(worldCard);
				}
			}
		}
	}

	public void OnAddToItemListPressed(int item)
	{
		GD.Print("BRUH");
		IItem createdItem = Items.CreateItemFromType((ItemType)item);
		if (!Utility.ItemListToNameList(ItemList).Contains(createdItem.Name)) ItemList.Add(createdItem);
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
}
