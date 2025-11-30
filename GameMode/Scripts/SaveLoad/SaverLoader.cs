using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

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
	public int gameDifficulty { get; set; }
}

public partial class SaverLoader : Node
{
	public const string SAVE_PATH = "user://Saves/Template/";
	public const string CONTINUE_PATH = "user://Saves/Continue/";
	public const string OUTPUT_PATH = "user://CreatedInFiles/";
	public WorldContext currSaveFile { get; set; }

	private static JsonSerializerOptions options = new JsonSerializerOptions
	{
		WriteIndented = true,
		TypeInfoResolver = new DefaultJsonTypeInfoResolver
		{
			Modifiers =
			{
				ti =>
				{
					if (ti.Type == typeof(Card))
					{
						ti.PolymorphismOptions = new JsonPolymorphismOptions
						{
							TypeDiscriminatorPropertyName = "$type",
							IgnoreUnrecognizedTypeDiscriminators = true,
							DerivedTypes =
							{
								new JsonDerivedType(typeof(BossCard), "boss")
							}
						};
					}
				}
			}
		}
	};

	public void SaveTo(WorldContext WorldContext)
	{
		string folder = $"{CONTINUE_PATH}{WorldContext.Name}";
		string fullPath = ProjectSettings.GlobalizePath(folder);
		if (!SaveFolderExists(WorldContext.Name, CONTINUE_PATH)) Directory.CreateDirectory(fullPath);

		PlayerSave playerData;
		WorldSave worldSave;
		Settings settingsData;

		Convert(WorldContext, out playerData, out worldSave, out settingsData);
		string playerJson = JsonSerializer.Serialize(playerData, options);
		string worldJson = JsonSerializer.Serialize(worldSave, options);
		string settingsJson = JsonSerializer.Serialize(settingsData, options);

		File.WriteAllText(Path.Combine(fullPath, "player.json"), playerJson);
		File.WriteAllText(Path.Combine(fullPath, "world.json"), worldJson);
		File.WriteAllText(Path.Combine(fullPath, "settings.json"), settingsJson);

		GD.Print("Save saved");
	}

	public static WorldContext CreateSave(string saveName, List<Card> cardsList, List<Dungeon> dungeonList, Player player)
	{
		string folder = $"{SAVE_PATH}{saveName}";
		string fullPath = ProjectSettings.GlobalizePath(folder);
		if (!SaveFolderExists(saveName, SAVE_PATH)) Directory.CreateDirectory(fullPath);

		PlayerSave playerData;
		WorldSave worldSave;
		Settings settingsData;

		Convert(saveName, cardsList, dungeonList, player, out playerData, out worldSave, out settingsData);

		string playerJson = JsonSerializer.Serialize<PlayerSave>(playerData, options);
		string worldJson = JsonSerializer.Serialize(worldSave, options);
		string settingsJson = JsonSerializer.Serialize(settingsData, options);

		File.WriteAllText(Path.Combine(fullPath, "player.json"), playerJson);
		File.WriteAllText(Path.Combine(fullPath, "world.json"), worldJson);
		File.WriteAllText(Path.Combine(fullPath, "settings.json"), settingsJson);

		GD.Print("Save saved");
		return new WorldContext(playerData, worldSave, settingsData);
	}

	public WorldContext Load(string saveName, string path)
	{
		string filePath = ProjectSettings.GlobalizePath($"{path}{saveName}/");
		if (!Directory.Exists(filePath))
			Directory.CreateDirectory(filePath);

		string playerJson = File.ReadAllText(Path.Combine(filePath, "player.json"));
		string worldJson = File.ReadAllText(Path.Combine(filePath, "world.json"));
		string settingsJson = File.ReadAllText(Path.Combine(filePath, "settings.json"));
		PlayerSave playerData = JsonSerializer.Deserialize<PlayerSave>(playerJson, options);
		WorldSave worldData = JsonSerializer.Deserialize<WorldSave>(worldJson, options);
		Settings settings = JsonSerializer.Deserialize<Settings>(settingsJson, options);

		return new WorldContext(playerData, worldData, settings);
	}

	public WorldContext Load(string path)
	{
		if (!Directory.Exists(path))
			Directory.CreateDirectory(path);

		string playerJson = File.ReadAllText(Path.Combine(path, "player.json"));
		string worldJson = File.ReadAllText(Path.Combine(path, "world.json"));
		string settingsJson = File.ReadAllText(Path.Combine(path, "settings.json"));
		PlayerSave playerData = JsonSerializer.Deserialize<PlayerSave>(playerJson, options);
		WorldSave worldData = JsonSerializer.Deserialize<WorldSave>(worldJson, options);
		Settings settings = JsonSerializer.Deserialize<Settings>(settingsJson, options);

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

	public static void WorldContextToInTxt(WorldContext worldContext, string path)
	{
		List<string> output = ConvertWorldContextToOutput(worldContext);
		string fullPath = ProjectSettings.GlobalizePath(path + "/in.txt");
		GD.Print("PATH" + path);
		GD.Print("FULLPATH" + fullPath);
		File.WriteAllLines(fullPath, output);
	}

	private static List<string> ConvertWorldContextToOutput(WorldContext worldContext)
	{
		List<string> output = new List<string>();
		List<Card> worldCards = worldContext.WorldCards;
		List<Dungeon> worldDungeons = worldContext.WorldDungeons;
		List<Card> bossCards = new List<Card>();
		Player plr = worldContext.player;

		foreach (Card card in worldCards)
		{
			if (card is BossCard)
			{
				bossCards.Add(card);
				continue;
			}
			output.Add($"uj kartya;{card.Name};{card.BaseDamage};{card.Health};{Utility.CardElementToString(card.CardElement)}");
		}

		output.Add("");

		foreach (BossCard card in bossCards)
		{
			output.Add($"uj vezer;{card.Name};{card.BaseCard.Name};{(card.Doubled == BossDouble.HEALTH ? "eletero" : "sebzes")};{Utility.CardElementToString(card.CardElement)}");
		}

		output.Add("");

		foreach (Dungeon dungeon in worldDungeons)
		{
			if (dungeon.DungeonType == DungeonTypes.big)
			{
				int lastIndex = dungeon.DungeonDeck.Count - 1;
				List<Card> deckExceptLast = lastIndex > 0 ? dungeon.DungeonDeck.GetRange(0, lastIndex) : new List<Card>();

				string deckString = Utility.WorldObjectListToString<Card>(deckExceptLast, ',');
				string lastCardName = lastIndex >= 0 ? dungeon.DungeonDeck[lastIndex].Name : "";

				output.Add($"uj kazamata;{dungeon.DungeonType};{dungeon.Name};{deckString};{lastCardName}");
			}
			else
				output.Add($"uj kazamata;{dungeon.DungeonType};{dungeon.Name};{Utility.WorldObjectListToString<Card>(dungeon.DungeonDeck, ',')};{(dungeon.DungeonReward == DungeonRewardTypes.health ? "eletero" : "sebzes")}");
		}

		output.Add("");

		output.Add("uj jatekos");

		output.Add("");

		foreach (Card card in plr.Collection)
		{
			output.Add($"felvetel gyujtemenybe;{card.Name}");
		}

		return output;
	}

	// public static LoadWorldObjects

	public static bool SaveFolderExists(string saveName, string path)
	{
		string fullPath = ProjectSettings.GlobalizePath($"{path}{saveName}");
		return Directory.Exists(fullPath);
	}

	public static bool SaveFolderExists(string fullPath)
	{
		return Directory.Exists(fullPath);
	}

	public static void DeleteSave(string name, string path)
    {
        string fullPath = ProjectSettings.GlobalizePath($"{path}{name}");
		if (!SaveFolderExists(fullPath))
			return;
		Directory.Delete(fullPath, true);
    }

	public static void DeleteFullPath(string name)
    {
        string fullPath = ProjectSettings.GlobalizePath($"{SAVE_PATH}{name}");
		if (!SaveFolderExists(fullPath))
			return;
		Directory.Delete(fullPath, true);
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
			gameDifficulty = 0
		};
	}

}
