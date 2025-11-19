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

	// TEST
	List<Card> PlayerDeck = new List<Card> { new Card("Corky", 2, 4, CardElements.EARTH), new Card("Kira", 2, 7, CardElements.WIND) };
	List<Card> DungeonDeck = new List<Card> { new Card("Sadan", 2, 4, CardElements.WIND) };
	List<IItem> ItemList = new List<IItem>();
	private int Round = 1;

	public RoundState SimulateRound(Card DungeonCard, Card PlayerCard, List<IItem> itemList)
	{
		foreach (IItem item in itemList)
		{
			item.ApplyPlayerBuff(PlayerCard);
			item.ApplyDungeonBuff(DungeonCard);
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
    }

	private void EndFight(string output)
	{
		GD.Print(output);
	}

	public void AddToItemList(IItem item)
    {
        ItemList.Add(item);
    }

}
