using Godot;
using System;

public partial class PiciMenü : Control
{
    [Export] Button HPButton;
    [Export] Button DMGButton;
    [Export] Button CollectionButton;

	public delegate void ClickedEventHandler(PiciMenü option);
	public event ClickedEventHandler Clicked;

    public Editors editor;

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

    public override void _Ready()
    {
        MouseExited += QueueFree; 
    }


	public void ButtonPressed(string option)
    {
		this.option = option;
        Clicked.Invoke(this);
		QueueFree();
    }

    private void DeleteItem()
    {
        if (Item is UICard)
        {
            editor.gameMasterData.RemoveCardFromWorldCards(((UICard)Item).CreateCardInstance());
        } else if (Item is UIDungeon)
        {
            editor.gameMasterData.RemoveDungeonFromDungeonList(((UIDungeon)Item).CreateDungeonInstance());
        }
        QueueFree();
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
