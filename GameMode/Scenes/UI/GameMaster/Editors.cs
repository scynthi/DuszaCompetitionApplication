using Godot;
using System.Threading.Tasks;

public partial class Editors : VBoxContainer
{
    public Editors()
    {
        Global.masterEditor = this;
    }

    [Export] public CardEditor CardEditor;
    [Export] public DungeonEditor DungeonEditor;
    [Export] public BossEditor BossEditor;
    [Export] public DungeonViewer DungeonViewer;
    [Export] public SaveMenu SaveMenu;
    [Export] public CardViewer CardViewer;
    [Export] public PlayerCollection PlayerCollection;
    [Export] RichTextLabel CurrentMenuMessage;

    public GameMasterData gameMasterData = new();


    private Control _currentMenu;
    public Control CurrentMenu
    {
        get {return _currentMenu;}
        private set
        {
            if (_currentMenu != null) CurrentMenu.Visible = false;

            _currentMenu = value;
            _currentMenu.Visible = true;
            if (_currentMenu.HasMethod("HandleDataChange"))
            {
                _currentMenu.Call("HandleDataChange");
            }
        }
    }
	public override void _Input(InputEvent @event)
	{
		PiciMenüHandler.HandlePiciMenüCreation(@event);
	}

    public override void _Ready()
    {
        foreach (Control child in GetChildren())
        {
            child.Visible = false;
        }
        CurrentMenu = CardEditor;
        EditMessage("[font_size=22]Kártya készítő[/font_size]\n\nItt tud kártyákat létrehozni és egyaránt személyre szabni. ");

    }

    public async void ChangeEditor(string name)
    {
        switch(name)
        {
            case "card":
                CurrentMenu = CardEditor;
                EditMessage("[font_size=22]Kártya készítő[/font_size]\n\nItt tud kártyákat létrehozni és egyaránt személyre szabni. ");
                break;
            case "dungeon":
                CurrentMenu = DungeonEditor;
                EditMessage("[font_size=22]Kazamata készítő[/font_size]\n\nItt tud kazamatákat létrehozni.");
                break;
            case "boss":
                CurrentMenu = BossEditor;
                EditMessage("[font_size=22]Vezér kártyák[/font_size]\n\nItt láthatja a játékkörnyezetben lévő vezér kártyákat. Bal kattintással meg tudja nyitni a 'Pici menüt', amely segítségével törölheti.");
                break;
            case "dungeons":
                CurrentMenu = DungeonViewer;
                EditMessage("[font_size=22]Kazamaták[/font_size]\n\nItt láthatja a játékkörnyezetben lévő kazamatákat. Bal kattintással meg tudja nyitni a 'Pici menüt', amely segítségével törölheti.");
                break;
            case "save":
                CurrentMenu = SaveMenu;
                EditMessage("[font_size=22]Mentés menü[/font_size]\n\nNe felejtsen el menteni!");
                break;
            case "cards":
                CurrentMenu = CardViewer;
                EditMessage("[font_size=22]Világkártyák[/font_size]\n\nItt láthatja a játékkörnyezetben lévő kártyákat. Bal kattintással meg tudja nyitni a 'Pici menüt', amely segítségével törölheti, hozzáadhatja a játékos gyűjteményéhez a kártyát vagy létrehozhat vezér kártyákat.");
                break;
            case "collection":
                CurrentMenu = PlayerCollection;
                EditMessage("[font_size=22]Gyűjtemény[/font_size]\n\nItt láthatja a játékos gyűjteményét.");
                break;
            case "main":
                EditMessage("[font_size=22]Csá![/font_size]");

                await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.MainMenu);
                break;
        }
    }

    private async void EditMessage(string message)
    {
        CurrentMenuMessage.VisibleRatio = 0;
        CurrentMenuMessage.Text = message;

        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(CurrentMenuMessage, new NodePath("visible_ratio"), 1.0, 1.0);
        tween.Play();
        await ToSignal(tween, "finished");
        tween.Kill();
    }

    // public void RemoveCard(string name)
    // {
    //     for (int i = 0; i < currentSaveFile.worldCards.Count; i++)
    //     {
    //         if (currentSaveFile.worldCards[i].Name == name)
    //         {
    //             currentSaveFile.worldCards.RemoveAt(i);
    //         }
    //     }
    // }

    // public void RemoveDungeon(string name)
    // {
    //     for (int i = 0; i < currentSaveFile.dungeons.Count; i++)
    //     {
    //         if (currentSaveFile.dungeons[i].Name == name)
    //         {
    //             currentSaveFile.dungeons.RemoveAt(i);
    //         }
    //     }
    // }

}
