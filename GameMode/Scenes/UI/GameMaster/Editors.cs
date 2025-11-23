using Godot;
using System;
using System.Collections.Generic;

public partial class Editors : VBoxContainer
{
    [Export] public CardEditor CardEditor;
    [Export] public DungeonEditor DungeonEditor;
    [Export] public BossEditor BossEditor;


    public List<Card> worldCards = new();
    public List<Card> playerCollection = new();
    public List<Card> dungeonsList = new();

    
    private Control _currentMenu;
    private Control CurrentMenu
    {
        get {return _currentMenu;}
        set
        {
            if (_currentMenu != null) CurrentMenu.Visible = false;

            _currentMenu = value;
            _currentMenu.Visible = true;
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
            case "main":
                await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.MainMenu);
                break;
        }
    }

}
