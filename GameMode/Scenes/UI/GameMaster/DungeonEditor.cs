using Godot;
using System;
using System.Linq;

public partial class DungeonEditor : HBoxContainer
{
    [Export] UIDungeon dungeon;
    Editors editor;

    public override void _Ready()
    {
        editor = (Editors)GetParent();
        editor.gameMasterData.DungeonDataChanged += HandleDataChange;
    }

    public void HandleDataChange() {}

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
        editor.gameMasterData.AddDungeonToDungeonList(dungeon.CreateDungeonInstance());
    }
}
