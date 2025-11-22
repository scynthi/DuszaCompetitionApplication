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
                await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.DungeonMap);
                break;
            case "settings":

                Card randCard = new Card("asd", 10, 5, CardElements.EARTH);

                
                SaveFileResource newSaveFile = SaveLoadSystem.CreateSaveFileFromData("testSaveSikibidi", 
                    [randCard],
                    [new Dungeon("Dungi", DungeonTypes.simple, DungeonRewardTypes.health)],
                    new Player(0,0,[randCard],[randCard])
                    );
                Global.gameManager.saverLoader.currSaveFile = newSaveFile;


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
