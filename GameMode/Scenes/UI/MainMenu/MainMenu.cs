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
                Card randCard = new Card("asd", 10, 5, CardElements.EARTH);
                Card rand1Card = new Card("asd1", 10, 5, CardElements.WATER);
                Card rand2Card = new Card("asd2", 10, 5, CardElements.WIND);
                Card rand3Card = new Card("asd3", 10, 5, CardElements.FIRE);
                Card rand4Card = new Card("asd4", 10, 5, CardElements.WATER);

                
                SaveFileResource newSaveFile = SaveLoadSystem.CreateSaveFileFromData("testSave2", 
                    [randCard,
                    rand1Card,
                    rand2Card,
                    rand3Card,
                    rand4Card
                    ],
                    [new Dungeon("Dungi", DungeonTypes.simple, DungeonRewardTypes.health),
                    new Dungeon("Dungi1", DungeonTypes.small, DungeonRewardTypes.attack),
                    new Dungeon("Dungi2", DungeonTypes.big, DungeonRewardTypes.health),
                    new Dungeon("Dungi3", DungeonTypes.simple, DungeonRewardTypes.health),
                    new Dungeon("Dungi4", DungeonTypes.simple, DungeonRewardTypes.health),
                    new Dungeon("Dungi5", DungeonTypes.simple, DungeonRewardTypes.health),
                    new Dungeon("Dungi6", DungeonTypes.simple, DungeonRewardTypes.health),
                    new Dungeon("Dungi7", DungeonTypes.simple, DungeonRewardTypes.health),
                    ],
                    new Player(0,0,[randCard],[randCard])
                    );

                Global.gameManager.saverLoader.WriteSaveFile(newSaveFile);

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
