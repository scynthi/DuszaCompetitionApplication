using Godot;
using System;

public partial class MainMenu : Control
{

    [Export] Control mainMenu;
    [Export] Control creditsMenu;

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
        CurrentMenu = mainMenu;
    }

    private void ButtonPressed(string option)
    {
        switch(option)
        {
            case "editor":
                GetTree().ChangeSceneToFile("res://Scenes/GameMaster/GameMaster.tscn");
                break;
            case "continue":
                //
                break;
            case "newgame":
                //
                break;
            case "settings":
                //
                break;
            case "credits":
                CurrentMenu = creditsMenu;
                break;
            case "quit":
                GetTree().Quit();
                break;
            case "main":
                CurrentMenu = mainMenu;
            break;
        }
    }
}
