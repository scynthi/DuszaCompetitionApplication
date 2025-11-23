using Godot;
using System;

public partial class DungeonViewer : HBoxContainer
{
	[Export] public Control dungeonHolder;
    [Export] PackedScene dungeonScene;

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse && mouse.Pressed && mouse.ButtonIndex == MouseButton.Left && Visible)
		{
			Control hoveredItem = GetViewport().GuiGetHoveredControl();

			if (hoveredItem == null) return;
			if (hoveredItem.Name.ToString() != "Dungeon") return;

			UIDungeon dungeon = (UIDungeon)hoveredItem.GetParent();

			if (dungeon == null) return;
			dungeon.QueueFree();
		}
	}

	public void AddDungeonToList(UIDungeon dungeon)
    {
        UIDungeon newDungeon = dungeonScene.Instantiate() as UIDungeon;
        dungeonHolder.AddChild(newDungeon);
        newDungeon.SetUpDungeon(dungeon);
    }
}
