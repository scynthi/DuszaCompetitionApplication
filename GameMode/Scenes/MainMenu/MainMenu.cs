using Godot;
using System;

public partial class MainMenu : Control
{
    private void MasteButtonPressed()
    {
        Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.MainMenu);
    }
}
