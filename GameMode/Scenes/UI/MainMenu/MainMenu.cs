using Godot;
using System;

public partial class MainMenu : Control
{
    [Export] Control MenuContainer;
    [Export] AnimationPlayer animationPlayer;
    [Export] AnimationPlayer sideAnimationPlayer;


    [ExportGroup("Menus")]
    [Export] Control mainMenu;
    [Export] Control creditsMenu;
    [Export] NewGameSubMenu newGameMenu;
    [Export] ContinueGameSubMenu continueGameMenu;
    [Export] SettingsMenu settingsMenu;


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

        _skipSound = true;

        ButtonPressed("main");
        sideAnimationPlayer.Play("BringInSideMenu");
    }

    private bool _skipSound = false;
    Control queuedMenu;
    public async void ButtonPressed(string option)
    {
        if (Global.gameManager != null && !_skipSound)
        {
            Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());
        }
        _skipSound = false;

        switch(option)
        {
            case "editor":
                await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.EditorMenu);
                break;
            case "continue":
                queuedMenu = continueGameMenu;
                continueGameMenu.ReloadSaves();
                break;
            case "newgame":
                queuedMenu = newGameMenu;
                newGameMenu.ReloadSaves();
                break;
            case "settings":
                queuedMenu = settingsMenu;
                settingsMenu.ReloadSliders();
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

    private void PlaySwooshSFX()
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.hoverSounds.PickRandom());
    }
    
    private void PlayChainSFX()
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.hoverSounds.PickRandom());

        // Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.chainSounds.PickRandom(), true, -.2f, -0.1f, 0.1f);
    }
}
