using Godot;
using System;
using System.Collections.Generic;


public partial class DungeonViewer : HBoxContainer
{
	[Export] public Control dungeonHolder;

	PiciMenü piciMenüInstance;
	Editors editor = Global.masterEditor;

    public override void _Ready()
    {
    	editor.gameMasterData.DungeonDataChanged += HandleDataChange;
    }

    public override void _ExitTree()
    {
		editor.gameMasterData.DungeonDataChanged -= HandleDataChange;
    }

	public void HandleDataChange()
    {
		if (!Visible) return;
        Utility.AddUiDungeonsUnderContainer(editor.gameMasterData.Dungeons, dungeonHolder);
    }
}
