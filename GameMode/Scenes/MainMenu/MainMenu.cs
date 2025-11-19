using Godot;
using System;

public partial class MainMenu : Control
{
    private void MasteButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/GameMaster/GameMaster.tscn");
    }
}
