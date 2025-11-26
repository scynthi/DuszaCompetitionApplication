using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public enum RoundState
{
	PLAYERDEATH,
	DUNGEONDEATH,
	NODEATH
}
public partial class FightLogic : Node
{
	// List<Card> PlayerDeck = new List<Card>();
	// List<Card> DungeonDeck = new List<Card>();
	// List<IItem> ItemList = new List<IItem>();

	[Export] RichTextLabel RoundText;
	[Export] HBoxContainer IconContainer;
	[Export] Control PlayerCard;
	[Export] Control EnemyCard;
	[Export] RichTextLabel DamageNum;
	[Export] AnimationPlayer AnimPlayer;

	// TEST
	List<Card> PlayerDeck = new List<Card> { new Card("Corky", 2, 4, CardElements.EARTH), new Card("Kira", 2, 7, CardElements.WIND) };
	List<Card> DungeonDeck = new List<Card> { new Card("Sadan", 2, 4, CardElements.WIND) };
	List<IItem> ItemList = new List<IItem>();
	private int Round = 1;
	Player player = Global.gameManager.saverLoader.currSaveFile.player;

	public override void _Ready()
	{
		foreach (Card card in player.Deck) PlayerDeck.Add(new Card(card));
		// foreach (Card card in Global.gameManager.saverLoader.currSaveFile.player.Deck) PlayerDeck.Add(new Card(card));
		LoadItemButtons(player.ItemList);
		LoadBattleItems();
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

	public void LoadBattleItems()
    {
        Utility.AddUiCardUnderContainer(PlayerDeck[0], PlayerCard);
        Utility.AddUiCardUnderContainer(DungeonDeck[0], EnemyCard);
    }

	public void DisplayAttack(int damage)
    {
        // DamageNum.Text = damage.ToString();
		AnimPlayer.Play("DisplayDamage");
    }

	public RoundState SimulateRound(Card DungeonCard, Card PlayerCard, List<IItem> itemList)
	{
		int damage; 
		foreach (IItem item in itemList)
		{
			item.ApplyPlayerBuff(PlayerCard, Round);
			item.ApplyDungeonBuff(DungeonCard, Round);
			player.RemoveFromItemList(item);
		}
		damage = DungeonCard.Attack(PlayerCard);
		GD.Print("Dungeon Attack: " + damage);
		DisplayAttack(damage);
		if (PlayerCard.Health == 0) return RoundState.PLAYERDEATH;

		damage = PlayerCard.Attack(DungeonCard);
		GD.Print("Player Attack: " + damage);
		DisplayAttack(damage);
		if (DungeonCard.Health == 0) return RoundState.DUNGEONDEATH;
		return RoundState.NODEATH;
	}

	private void OnButtonPressed()
	{
		RoundText.Text = $"Round: {Round}";
		if (PlayerDeck.Count > 0 && DungeonDeck.Count > 0)
		{
			Card playerCard = PlayerDeck[0];
			Card dungeonCard = DungeonDeck[0];
			playerCard.buffHandler.ClearBuffs(Round);
			dungeonCard.buffHandler.ClearBuffs(Round);
			RoundState currRound = SimulateRound(dungeonCard, playerCard, ItemList);
			ItemList.Clear();
			if (currRound == RoundState.PLAYERDEATH)
			{
				PlayerDeck.RemoveAt(0);
				GD.Print($"{playerCard.Name} player card died");
			}
			else if (currRound == RoundState.DUNGEONDEATH)
			{
				DungeonDeck.RemoveAt(0);
				GD.Print($"{dungeonCard.Name} enemy card died");
			}
			Round++;
			return;
		}
		EndFight(PlayerDeck.Count > 0 ? $"Player Won {PlayerDeck[0].Name}, {PlayerDeck[0].Health}" : $"Enemy Won {DungeonDeck[0].Name}, {DungeonDeck[0].Health}");
	}

	private void OnItemPressed()
	{
		GD.Print("BRUH");
		GD.Print("pront");
	}

	private void EndFight(string output)
	{
		GD.Print(output);
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
