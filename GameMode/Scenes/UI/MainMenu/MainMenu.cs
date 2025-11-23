using Godot;
using System;

public partial class MainMenu : Control
{

    [Export] Control mainMenu;
    [Export] Control creditsMenu;
    [Export] NewGameSubMenu newGameMenu;
    [Export] Control MenuContainer;

    [Export] AnimationPlayer animationPlayer;

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
        foreach (Control control in MenuContainer.GetChildren())
        {
            control.Visible = false;
        }

        CurrentMenu = mainMenu;
    }

    Control queuedMenu;
    public async void ButtonPressed(string option)
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());

        switch(option)
        {
            case "editor":
                await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.EditorMenu);
                break;
            case "continue":
                break;
            case "newgame":
                queuedMenu = newGameMenu;
                newGameMenu.ReloadSaves();
                break;
            case "settings":
                break;
            case "credits":
                queuedMenu = creditsMenu;
                break;
            case "quit":
                GetTree().Quit();
                break;
            case "main":
                queuedMenu = mainMenu;
            break;
        }

        animationPlayer.Play("ChangeSubMenu");
    }

    private void ChangeMenu()
    {
        CurrentMenu = queuedMenu;
    }
}
