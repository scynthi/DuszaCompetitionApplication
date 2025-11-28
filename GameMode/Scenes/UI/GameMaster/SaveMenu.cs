using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SaveMenu : HBoxContainer
{
	[Export] LineEdit saveNameInput;
	Editors editor = Global.masterEditor;

    public override void _Ready()
    {
		if (editor.gameMasterData.SaveName != null) saveNameInput.Text = editor.gameMasterData.SaveName;
    }

	public void CreateSave()
    {
        string saveName = saveNameInput.Text.ToString();
		// TODO: make more check
		if (saveName.Replace(" ", "") == "") {
			GD.Print("Bad file name!");
			return;
		}

		SaverLoader.CreateSave(
			saveName, 
			editor.gameMasterData.WorldCards,
			editor.gameMasterData.Dungeons,
			new Player(0, 0, editor.gameMasterData.PlayerCollection)
		);

		GD.Print($"Save file ({saveName}) has been saved by InterPeter's hyper super backend of Saving files sponsored by Godot and God himself.");
		GD.Print($"Peti this in fact doesn't work, and it wasn't so hyper super");
		GD.Print($"Peti? PETI? HAHAH!! YOU FOOL! IT WAS ME, DANI ALL ALONG! I seriously can't believe the fact that you thought Peti would code a terrible system like this. ");
    }
}
