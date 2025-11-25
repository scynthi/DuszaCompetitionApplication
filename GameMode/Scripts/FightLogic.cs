using Godot;
using System;
using System.Collections.Generic;

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

	// TEST
	List<Card> PlayerDeck = new List<Card> { new Card("Corky", 2, 4, CardElements.EARTH), new Card("Kira", 2, 7, CardElements.WIND) };
	List<Card> DungeonDeck = new List<Card> { new Card("Sadan", 2, 4, CardElements.WIND) };
	List<IItem> ItemList = new List<IItem> { };
	private int Round = 1;

	public override void _Ready()
	{
		foreach (Card card in Global.gameManager.saverLoader.currSaveFile.player.Deck) PlayerDeck.Add(new Card(card));
		foreach (Card card in Global.gameManager.saverLoader.currSaveFile.player.Deck) PlayerDeck.Add(new Card(card));
		foreach (Card card in Global.gameManager.saverLoader.currSaveFile.player.Deck) PlayerDeck.Add(new Card(card));
	}

	public void LoadItemButtons(List<IItem> itemList)
    {
        PackedScene iconButtonScene = GD.Load<PackedScene>("uid://duqvkh3tdlkve");

		foreach (IItem item in itemList)
		{
			ItemButton btn = iconButtonScene.Instantiate<ItemButton>();
			btn.itemType = item.Type;
			btn.Send_Item += OnAddToItemListPressed;
			IconContainer.AddChild(btn);
		}
    }

	public RoundState SimulateRound(Card DungeonCard, Card PlayerCard, List<IItem> itemList)
	{
		foreach (IItem item in itemList)
		{
			item.ApplyPlayerBuff(PlayerCard, Round);
			item.ApplyDungeonBuff(DungeonCard, Round);
		}

		GD.Print("Dungeon Attack: " + DungeonCard.Attack(PlayerCard));
		if (PlayerCard.Health == 0) return RoundState.PLAYERDEATH;

		GD.Print("Player Attack: " + PlayerCard.Attack(DungeonCard));
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
}
