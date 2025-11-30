using Godot;
using System;

public partial class StartSplashScreen : Control
{

    public async void GoToGame()
    {
       await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.MainMenu);
    }
}
