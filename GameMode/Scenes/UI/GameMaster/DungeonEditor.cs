using Godot;
using System;

public partial class DungeonEditor : HBoxContainer
{
    [Export] UIDungeon dungeon;

    public void ChangeName(string text)
    {
        dungeon.EditName(text);
    }

    public void ChangeType(int index)
    {
        dungeon.EditType((DungeonTypes)index);
    }
    
    // TODO: rewrite it when backend arrives
    // TODO: Do more checks for name
    public void SaveDungeon()
    {
        ((Editors)GetParent()).DungeonViewer.AddDungeonToList(dungeon);
    }
}
