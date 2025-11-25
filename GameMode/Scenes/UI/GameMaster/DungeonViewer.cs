using Godot;
using System;
using System.Collections.Generic;


public partial class DungeonViewer : HBoxContainer
{
	[Export] public Control dungeonHolder;

	PiciMenü piciMenüInstance;
	Editors editor;

    public override void _Ready()
    {
        editor = (Editors)GetParent();
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

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse && mouse.Pressed && mouse.ButtonIndex == MouseButton.Left && Visible)
		{
			Control hoveredItem = GetViewport().GuiGetHoveredControl();

			if (hoveredItem == null) return;
			GD.Print(hoveredItem);

			if (hoveredItem.Name.ToString() != "Dungeon") return;

			UIDungeon dungeon = (UIDungeon)hoveredItem.GetParent();

			if (dungeon == null) return;

			piciMenüInstance = CreatePiciMenü();
			piciMenüInstance.Item = dungeon;
		}
	}

	private PiciMenü CreatePiciMenü()
	{
		PiciMenü piciMenüInstance = Global.gameManager.uiPackedSceneReferences.PiciMenü.Instantiate<PiciMenü>();

		var uiRoot = GetTree().CurrentScene.GetNode("GameLayer/World");
		uiRoot.AddChild(piciMenüInstance);

		piciMenüInstance.editor = editor;
		piciMenüInstance.ZIndex = 999;
		piciMenüInstance.MouseFilter = MouseFilterEnum.Stop;
		piciMenüInstance.GlobalPosition = GetGlobalMousePosition();

		return piciMenüInstance;
	}
}
