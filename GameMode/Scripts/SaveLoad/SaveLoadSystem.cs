using Godot;
using System;
using System.Collections.Generic;

public partial class SaveLoadSystem : Node
{
	public WorldContext currSaveFile { get; set; }

	public const string SAVE_PATH = "res://Saves/";

	public static WorldContext CreateSaveFileFromData(string saveFileName, List<Card> worldCardsList, List<Dungeon> dungeonList, List<Card> playerCollection)
    {
		Player newPlayer = new Player(0, 0);
		newPlayer.SetCollection(playerCollection);

		return new WorldContext(saveFileName, newPlayer, worldCardsList, dungeonList);
    }

	public static WorldContext CreateSaveFileFromData(string saveFileName, List<Card> worldCardsList, List<Dungeon> dungeonList, Player playerInstance)
    {
		return new WorldContext(saveFileName, playerInstance, worldCardsList, dungeonList);
    }



	public void WriteSaveFile(WorldContext saveFile)
    {
        // ResourceSaver.Save(saveFile, SAVE_PATH + $"{saveFile.Name}.tres");
    }

	public WorldContext LoadSaveFile(string saveFileName)
    {
        if (ResourceLoader.Exists(SAVE_PATH + $"{saveFileName}"))
        {
            return GD.Load<WorldContext>(SAVE_PATH + $"{saveFileName}");
        } 
		return null;
    }

}
