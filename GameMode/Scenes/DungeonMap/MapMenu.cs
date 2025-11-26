using Godot;
using System;

public partial class MapMenu : Control
{
    [Export] Control submenuContainer;
    [Export] DeckSubmenu deckMenu;
    [Export] WorldCardsSubmenu worldCardsMenu;
    [Export] Control emptyMenu;

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
        foreach (Control child in submenuContainer.GetChildren())
        {
            child.Visible = false;
        }
    }


    Control queuedMenu;
    string lastMenuName;

    public async void ChangeSubMenu(string name)
    {
        if (lastMenuName == name)
        {
            queuedMenu = emptyMenu;
            animationPlayer.Play("ChangeSubmenu");
            lastMenuName = null;
            return;
        }

        switch(name)
        {
            case "deckmenu":
                queuedMenu = deckMenu;
                lastMenuName = name;
                deckMenu.ReloadCards();
                animationPlayer.Play("ChangeSubmenu");
                break;
            case "cardsmenu":
                queuedMenu = worldCardsMenu;
                lastMenuName = name;
                worldCardsMenu.ReloadWorldCards();
                animationPlayer.Play("ChangeSubmenu");
                GD.Print("as");
                break;
            case "shop":
                await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.ShopMenu);
                break;
            case "main":
                Global.gameManager.saverLoader.SaveTo(Global.gameManager.saverLoader.currSaveFile);
                await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.MainMenu);
                break;
        }
    }

    private void ChangeMenu()
    {
        CurrentMenu = queuedMenu;
    }

}
