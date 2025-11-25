using Godot;
using System;
using System.Collections.Generic;

public partial class Editors : VBoxContainer
{
    [Export] public CardEditor CardEditor;
    [Export] public DungeonEditor DungeonEditor;
    [Export] public BossEditor BossEditor;
    [Export] public DungeonViewer DungeonViewer;
    [Export] public SaveMenu SaveMenu;

    public GameMasterData gameMasterData = new();


    private Control _currentMenu;
    private Control CurrentMenu
    {
        get {return _currentMenu;}
        set
        {
            if (_currentMenu != null) CurrentMenu.Visible = false;

            _currentMenu = value;
            _currentMenu.Visible = true;
            if (_currentMenu.HasMethod("HandleDataChange"))
            {
                _currentMenu.Call("HandleDataChange");
            }
        }
    }

    public override void _Ready()
    {
        foreach (Control child in GetChildren())
        {
            child.Visible = false;
        }
        CurrentMenu = CardEditor;
    }

    public async void ChangeEditor(string name)
    {
        switch(name)
        {
            case "card":
                CurrentMenu = CardEditor;
                break;
            case "dungeon":
                CurrentMenu = DungeonEditor;
                break;
            case "boss":
                CurrentMenu = BossEditor;
                break;
            case "dungeons":
                CurrentMenu = DungeonViewer;
                break;
            case "save":
                CurrentMenu = SaveMenu;
                break;
            case "main":
                await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.MainMenu);
                break;
        }
    }

    // public void RemoveCard(string name)
    // {
    //     for (int i = 0; i < currentSaveFile.worldCards.Count; i++)
    //     {
    //         if (currentSaveFile.worldCards[i].Name == name)
    //         {
    //             currentSaveFile.worldCards.RemoveAt(i);
    //         }
    //     }
    // }

    // public void RemoveDungeon(string name)
    // {
    //     for (int i = 0; i < currentSaveFile.dungeons.Count; i++)
    //     {
    //         if (currentSaveFile.dungeons[i].Name == name)
    //         {
    //             currentSaveFile.dungeons.RemoveAt(i);
    //         }
    //     }
    // }

}
