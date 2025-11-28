using Godot;
using System;
using System.Collections.Generic;

public partial class Dungeon : IWorldObject
{
	public string Name { set; get; }
	public DungeonTypes DungeonType { set; get; }
	public DungeonRewardTypes DungeonReward { set; get; }
    public List<Card> DungeonDeck { set; get; }

    public Dungeon(){}

	public Dungeon(string name, DungeonTypes dungeonType, DungeonRewardTypes dungeonReward, List<Card> dungeonDeck)
    {
        Name          = name;
        DungeonType   = dungeonType;
        DungeonReward = dungeonReward;
        DungeonDeck   = dungeonDeck;
    }

	public Dungeon(Dungeon other)
    {
        Name   		  = other.Name;
        DungeonType   = other.DungeonType;
        DungeonReward = other.DungeonReward;
        DungeonDeck   = other.DungeonDeck;
    }

	public void SetDungeonDeck(List<Card> cardList)
    {
		DungeonDeck.Clear();
        foreach (Card card in cardList) DungeonDeck.Add(new Card(card));
    }
}
