using Godot;
using System;

public partial class DungeonViewer : HBoxContainer
{
	[Export] HBoxContainer dungeonHolder;

	public void AddDungeonToList(UIDungeon dungeon)
    {
        UIDungeon newDungeon = new();
        newDungeon.SetUpDungeon(dungeon);
        dungeonHolder.AddChild(newDungeon);
    }
}
