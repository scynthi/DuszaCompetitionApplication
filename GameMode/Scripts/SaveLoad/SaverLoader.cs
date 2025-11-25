using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class PlayerSave
{
	public Player player { get; set; }
	
}

public class WorldSave
{
    public List<Card> WorldCards { get; set; }
	public List<Dungeon> WorldDungeons { get; set; }
}

public class Settings
{
	public string Name { get; set; }
    public bool isStarted { get; set; }
	public int gameDifficulty { get; set; }
}

public partial class SaverLoader : Node
{
	public const string SAVE_PATH = "res://Saves/";
	public WorldContext currSaveFile { get; set; }

	public void SaveTo(WorldContext WorldContext)
	{
		string folder = $"{SAVE_PATH}{WorldContext.Name}";
		string fullPath = ProjectSettings.GlobalizePath(folder);
		if (!SaveFolderExists(WorldContext.Name)) Directory.CreateDirectory(fullPath);

		PlayerSave playerData;
		WorldSave worldSave;
		Settings settingsData;

		Convert(WorldContext, out playerData, out worldSave, out settingsData);

		string playerJson = JsonSerializer.Serialize(playerData, new JsonSerializerOptions
		{
			WriteIndented = true
		});
		string worldJson = JsonSerializer.Serialize(worldSave, new JsonSerializerOptions
		{
			WriteIndented = true
		});
		string settingsJson = JsonSerializer.Serialize(settingsData, new JsonSerializerOptions
		{
			WriteIndented = true
		});

		File.WriteAllText(Path.Combine(fullPath, "player.json"), playerJson);
		File.WriteAllText(Path.Combine(fullPath, "world.json"), worldJson);
		File.WriteAllText(Path.Combine(fullPath, "settings.json"), settingsJson);

		GD.Print("Save saved");
	}

	public static WorldContext CreateSave(string saveName, List<Card> cardsList, List<Dungeon> dungeonList, Player player)
	{
		string folder = $"{SAVE_PATH}{saveName}";
		string fullPath = ProjectSettings.GlobalizePath(folder);
		if (!SaveFolderExists(saveName)) Directory.CreateDirectory(fullPath);

		PlayerSave playerData;
		WorldSave worldSave;
		Settings settingsData;

		Convert(saveName, cardsList, dungeonList, player, out playerData, out worldSave, out settingsData);

		string playerJson = JsonSerializer.Serialize(playerData, new JsonSerializerOptions
		{
			WriteIndented = true
		});
		string worldJson = JsonSerializer.Serialize(worldSave, new JsonSerializerOptions
		{
			WriteIndented = true
		});
		string settingsJson = JsonSerializer.Serialize(settingsData, new JsonSerializerOptions
		{
			WriteIndented = true
		});

		File.WriteAllText(Path.Combine(fullPath, "player.json"), playerJson);
		File.WriteAllText(Path.Combine(fullPath, "world.json"), worldJson);
		File.WriteAllText(Path.Combine(fullPath, "settings.json"), settingsJson);

		GD.Print("Save saved");
		return new WorldContext(playerData, worldSave, settingsData);
	}

	public WorldContext Load(string saveName)
    {
		string filePath = ProjectSettings.GlobalizePath($"{SAVE_PATH}{saveName}/");
		GD.Print("OMD", filePath);

		if (!Directory.Exists(filePath))
			return null;

		GD.Print("WHY");
		string playerJson = File.ReadAllText(Path.Combine(filePath, "player.json"));
		string worldJson = File.ReadAllText(Path.Combine(filePath, "world.json"));
		string settingsJson = File.ReadAllText(Path.Combine(filePath, "settings.json"));
		PlayerSave playerData = JsonSerializer.Deserialize<PlayerSave>(playerJson);
		WorldSave worldData = JsonSerializer.Deserialize<WorldSave>(worldJson);
		Settings settings = JsonSerializer.Deserialize<Settings>(settingsJson);

		return new WorldContext(playerData, worldData, settings);
    }

	public static Player LoadPlayer(string saveName)
	{
		string filePath = ProjectSettings.GlobalizePath($"user://Saves/{saveName}/player.json");

		if (!File.Exists(filePath))
			return null;

		string json = File.ReadAllText(filePath);
		return JsonSerializer.Deserialize<Player>(json);
	}

	// public static LoadWorldObjects

	public static bool SaveFolderExists(string saveName)
	{
		string fullPath = ProjectSettings.GlobalizePath($"{SAVE_PATH}{saveName}");
		return Directory.Exists(fullPath);
	}

	public static void Convert(WorldContext r, out PlayerSave pSave, out WorldSave wSave, out Settings settings)
    {
        pSave = new PlayerSave
		{
			player = r.player
		};
		wSave = new WorldSave
		{
			WorldCards = r.WorldCards,
			WorldDungeons = r.WorldDungeons
		};
		settings = new Settings
		{
			Name = r.Name,
			isStarted = r.IsStarted,
			gameDifficulty = r.gameDifficulty
		};
    }

	public static void Convert(string saveName, List<Card> cardsList, List<Dungeon> dungeonList, Player player, out PlayerSave pSave, out WorldSave wSave, out Settings settings)
    {
        pSave = new PlayerSave
		{
			player = player
		};
		wSave = new WorldSave
		{
			WorldCards = cardsList,
			WorldDungeons = dungeonList
		};
		settings = new Settings
		{
			Name = saveName,
			isStarted = false,
			gameDifficulty = 0
		};
    }
}
