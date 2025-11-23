using Godot;
using System;
using System.Collections.Generic;

public partial class Dungeon : Resource, IWorldObject
{
	public new string Name { private set; get; }
	public DungeonTypes DungeonType {private set; get; }
	public DungeonRewardTypes DungeonReward { private set; get; }

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
}
