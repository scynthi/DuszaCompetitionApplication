using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

public partial class WorldContext : Node
{
	// public static WorldContext Instance { get; set; }
	public new string Name { private set; get; }
	public int gameDifficulty;
    public Player player { private set; get; }
    public List<Card> WorldCards { private set; get; } = new List<Card>();
    public List<Dungeon> WorldDungeons { private set; get; } = new List<Dungeon>();

	public string currDungeonName = "";

    // public override void _Ready()
    // {
    //     Instance = this;
    // }

    public WorldContext(List<Card> playerCollection, List<Card> worldCardsList, List<Dungeon> dungeonList)
    {
		player = new Player();
        player.Collection = playerCollection;
		WorldCards = worldCardsList;
		WorldDungeons = dungeonList;
    }


	public WorldContext(string saveFileName, Player playerInstance, int difficulty, List<Card> worldCardsList, List<Dungeon> dungeonList)
    {
        Name = saveFileName;
		player = playerInstance;
		AddSaveDeckToRealDeck(player);
        gameDifficulty = difficulty;
		WorldCards = worldCardsList;
		WorldDungeons = dungeonList;
    }

	public WorldContext(PlayerSave pSave, WorldSave wSave, Settings settings)
    {
        Name = settings.Name;
		gameDifficulty = settings.gameDifficulty;
		player = pSave.player;
		AddSaveDeckToRealDeck(player);
		WorldCards = wSave.WorldCards;
		WorldDungeons = wSave.WorldDungeons;
    }

	private void AddSaveDeckToRealDeck(Player player)
    {
        List<string> deckNames = Utility.WorldObjectListToNameList(player.Deck);
		player.Deck.Clear();
		foreach (string name in deckNames)
        {
            player.TryAddToDeck(name);
        }
    }
}
