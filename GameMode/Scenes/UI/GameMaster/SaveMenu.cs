using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SaveMenu : HBoxContainer
{
	[Export] LineEdit saveNameInput;
	Editors editor;

    public override void _Ready()
    {
        editor = (Editors)GetParent();
    }



	public void CreateSave()
    {
        string saveName = saveNameInput.Text.ToString();

		// TODO: make more check
		if (saveName == "") {
			GD.Print("Bad file name!");
			return;
		}

		// Editors editor = (Editors)GetParent();
		// Control UIBossCards = editor.BossEditor.BossCardHolder;
		// Control UISimpleCards = editor.BossEditor.NormalCardHolder;
		// Control UIDungeons = editor.DungeonViewer.dungeonHolder;

		// UIBossCard[] UIBossCardsArray = UIBossCards.GetChildren().OfType<UIBossCard>().ToArray();
		// UICard[] UISimpleCardsArray = UISimpleCards.GetChildren().OfType<UICard>().ToArray();
		// UIDungeon[] UIDungeonArray = UIDungeons.GetChildren().OfType<UIDungeon>().ToArray();

		// List<Card> cardsList = new();
		// List<Dungeon> dungeonList = new();


		// foreach (UICard uicard in UISimpleCardsArray)
		// {
		// 	Card card = uicard.CreateCardInstance();
		// 	cardsList.Add(card);
		// }

		// foreach (UIBossCard uicard in UIBossCardsArray)
		// {
		// 	Card card = uicard.CreateBossCardInstance();
		// 	cardsList.Add(card);
		// }

		// foreach (UIDungeon uidungeon in UIDungeonArray)
		// {
		// 	Dungeon dungeon = uidungeon.CreateDungeonInstance();
		// 	dungeonList.Add(dungeon);
		// }

		WorldContext saveFile = SaverLoader.CreateSave(
			saveName, 
			editor.gameMasterData.WorldCards,
			editor.gameMasterData.Dungeons,
			new Player(0, 0)
		);

		GD.Print($"Save file ({saveName}) has been saved by InterPeter's hyper super backend of Saving files sponsored by Godot and God himself.");
		GD.Print($"Peti this in fact doesn't work, and it wasn't so hyper super");
		GD.Print($"Peti? PETI? HAHAH!! YOU FOOL! IT WAS ME, DANI ALL ALONG! I seriously can't believe the fact that you thought Peti would code a terrible system like this. ");
    }
}
