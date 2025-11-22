using Godot;
using System;

public partial class Editors : VBoxContainer
{
    [Export] HBoxContainer CardEditor;
    [Export] HBoxContainer KazamataEditor;
    
    private HBoxContainer _currentMenu;
    private HBoxContainer CurrentMenu
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
        CurrentMenu = CardEditor;
    }

    public void ChangeEditor(string name)
    {
        switch(name)
        {
            case "card":
                CurrentMenu = CardEditor;
                break;
            case "kazamata":
                CurrentMenu = KazamataEditor;
                break;
            case "main":
                GetTree().ChangeSceneToFile("res://Scenes/MainMenu/MainMenu.tscn");
                break;
        }
    }

}
