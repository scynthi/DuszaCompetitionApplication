using Godot;
using System;
using System.Collections.Generic;

public partial class WorldContext : Node
{
	// public static WorldContext Instance { get; set; }
	public new string Name { private set; get; }
    public bool IsStarted { set; get; }
	public int gameDifficulty;
    public Player player { private set; get; }
    public List<Card> WorldCards { private set; get; } = new List<Card>();
    public List<Dungeon> WorldDungeons { private set; get; } = new List<Dungeon>();

	public string currDungeonName = "";

    // public override void _Ready()
    // {
    //     Instance = this;
    // }


	public WorldContext(string saveFileName, Player playerInstance, List<Card> worldCardsList, List<Dungeon> dungeonList)
    {
        Name = saveFileName;
		IsStarted = false;
		gameDifficulty = 0;
		player = playerInstance;
		WorldCards = worldCardsList;
		WorldDungeons = dungeonList;
    }

	public WorldContext(PlayerSave pSave, WorldSave wSave, Settings settings)
    {
        Name = settings.Name;
		IsStarted = settings.isStarted;
		gameDifficulty = settings.gameDifficulty;
		player = pSave.player;
		WorldCards = wSave.WorldCards;
		WorldDungeons = wSave.WorldDungeons;
    }
}
