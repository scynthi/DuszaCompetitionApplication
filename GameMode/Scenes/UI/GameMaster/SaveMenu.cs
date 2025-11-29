using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SaveMenu : HBoxContainer
{
	[Export] LineEdit saveNameInput;
	[Export] Control warningMenu;

	Editors editor = Global.masterEditor;


    public override void _Ready()
    {
		warningMenu.Visible = false;
		if (editor.gameMasterData.SaveName != null) saveNameInput.Text = editor.gameMasterData.SaveName;
    }

	string _CurrSaveName = "";

	public void CreateSave()
    {
		Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());


        string saveName = saveNameInput.Text.ToString();
		_CurrSaveName = saveName;
		// TODO: make more check
		if (saveName.Replace(" ", "") == "") {
			GD.Print("Bad file name!");
			return;
		}

		if (SaverLoader.SaveFolderExists(saveName, SaverLoader.SAVE_PATH))
        {
			warningMenu.Visible = true;
            return;
        }
		
		Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.levelupSounds.PickRandom());

		SaverLoader.CreateSave(
			saveName, 
			editor.gameMasterData.WorldCards,
			editor.gameMasterData.Dungeons,
			new Player(0, 0, editor.gameMasterData.PlayerCollection)
		);
	}

	public void CreatAnyways()
    {
		Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());
		Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.levelupSounds.PickRandom());

		SaverLoader.CreateSave(
			_CurrSaveName, 
			editor.gameMasterData.WorldCards,
			editor.gameMasterData.Dungeons,
			new Player(0, 0, editor.gameMasterData.PlayerCollection)
		);

		warningMenu.Visible = false;
		_CurrSaveName = "";
    }

	
	public void CreatAbort()
    {
		Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());

        warningMenu.Visible = false;
		_CurrSaveName = "";
    }
}
