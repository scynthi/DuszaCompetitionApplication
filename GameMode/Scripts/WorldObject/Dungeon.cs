using Godot;
using System;
using System.Collections.Generic;

public partial class Dungeon : Resource, IWorldObject
{
	[Export] public string Name { private set; get; }
	[Export] public DungeonTypes DungeonType {private set; get; }
	[Export] public DungeonRewardTypes DungeonReward { private set; get; }
    [Export] public Godot.Collections.Array<Card> DungeonDeck { private set; get; }

    public Dungeon(){}

	public Dungeon(string name, DungeonTypes dungeonType, DungeonRewardTypes dungeonReward)
    {
        Name          = name;
        DungeonType   = dungeonType;
        DungeonReward = dungeonReward;
    }

	public Dungeon(Dungeon other)
    {
        Name   		  = other.Name;
        DungeonType   = other.DungeonType;
        DungeonReward = other.DungeonReward;
    }

	public void SetDungeonDeck(List<Card> cardList)
    {
		DungeonDeck.Clear();
        foreach (Card card in cardList) DungeonDeck.Add(new Card(card));
    }
}
