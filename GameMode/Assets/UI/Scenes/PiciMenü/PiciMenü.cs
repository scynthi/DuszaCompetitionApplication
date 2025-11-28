using Godot;
using System;

public partial class PiciMenü : Control
{
    [Export] Button HPButton;
    [Export] Button DMGButton;
    [Export] Button CollectionButton;

	public delegate void ClickedEventHandler(PiciMenü option);
	public event ClickedEventHandler Clicked;

    public Editors editor = Global.masterEditor;

	private Control _item;
	public Control Item
    {
        get {return _item;}
		set
        {
			_item = value;
			UpdateButtons();
        }
    }
	public string option;

    public override void _Process(double delta)
    {
        if (!GetGlobalRect().HasPoint(GetGlobalMousePosition()))
        {
            QueueFree();
        }
    }

    public void AddToCollection()
    {
        QueueFree();
        // Todo check for duplicate, if duplicate disable add to collection option
        if (Item is UICard)
        {
            editor.gameMasterData.AddCardToPlayerCollection(((UICard)Item).CreateCardInstance());   
        }
    }

	public void ButtonPressed(string option)
    {
        QueueFree();
		this.option = option;
        Clicked.Invoke(this);
    }

    private void DeleteItem()
    {
        QueueFree();
        if (Item is UICard)
        {
            if (editor.CurrentMenu is PlayerCollection)
            {
                editor.gameMasterData.RemoveCardFromPlayerCollection(((UICard)Item).CreateCardInstance());
                return;
            }
            
            editor.gameMasterData.RemoveCardFromWorldCards(((UICard)Item).CreateCardInstance());

        } else if (Item is UIDungeon)
        {
            editor.gameMasterData.RemoveDungeonFromDungeonList(((UIDungeon)Item).CreateDungeonInstance());

        } else if (Item is UIBossCard)
        {
            editor.gameMasterData.RemoveCardFromWorldCards(((UIBossCard)Item).CreateBossCardInstance());

        }
    }

	private void UpdateButtons()
    {
        if (Item is UICard)
        {
            CollectionButton.Disabled = false;
            HPButton.Disabled = false;
            DMGButton.Disabled = false;
        } else if (Item is UIBossCard)
        {
            CollectionButton.Disabled = true;
            HPButton.Disabled = true;
            DMGButton.Disabled = true;
        } else if (Item is UIDungeon)
        {
            CollectionButton.Disabled = true;
            HPButton.Disabled = true;
            DMGButton.Disabled = true;
        }
    }

}
