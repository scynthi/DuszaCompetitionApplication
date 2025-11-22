using Godot;
using System;
using System.Collections.Generic;

public partial class SaveLoadSystem : Node
{
	public SaveFileResource currSaveFile {get; set;}

	public const string SAVE_PATH = "user://SaveFiles/";

	public static SaveFileResource CreateSaveFileFromData(string saveFileName, List<Card> worldCardsList, List<Dungeon> dungeonList, List<Card> playerCollection)
    {
		Player newPlayer = new Player(0,0, playerCollection, []);		
		newPlayer.SetCollection(playerCollection);

		return new SaveFileResource(saveFileName, newPlayer, worldCardsList, dungeonList);
    }

	public static SaveFileResource CreateSaveFileFromData(string saveFileName, List<Card> worldCardsList, List<Dungeon> dungeonList, Player playerInstance)
    {
		return new SaveFileResource(saveFileName, playerInstance, worldCardsList, dungeonList);
    }



	public void WriteSaveFile(SaveFileResource saveFile)
    {
        ResourceSaver.Save(saveFile, SAVE_PATH + $"{saveFile.name}");
    }

	public SaveFileResource LoadSaveFile(string saveFileName)
    {
        if (ResourceLoader.Exists(SAVE_PATH + $"{saveFileName}"))
        {
            return GD.Load<SaveFileResource>(SAVE_PATH + $"{saveFileName}");
        } 
		return null;
        
    }

}
