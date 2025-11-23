using Godot;
using System;

public partial class DungeonEditor : HBoxContainer
{
    [Export] UIDungeon dungeon;

    public void ChangeName(string text)
    {
        if (text.Replace(" ", "") == "") return;

        dungeon.EditName(text);
    }

    public void ChangeType(int index)
    {
        dungeon.EditType((DungeonTypes)index);
    }
    
    // TODO: rewrite it when backend arrives
    public void SaveKazamata()
    {
        GD.Print($"{dungeon.DungeonName}, {dungeon.DungeonType}");
    }
}
