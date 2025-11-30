using Godot;
using System;

public partial class PiciMenü : Control
{
    [Export] Button HPButton;
    [Export] Button DMGButton;
    [Export] Button CollectionButton;
    [Export] public LineEdit PrefixName;

    public delegate void ClickedEventHandler(PiciMenü option);
    public event ClickedEventHandler Clicked;

    public Editors editor = Global.masterEditor;

    private Control _item;
    public Control Item
    {
        get { return _item; }
        set
        {
            _item = value;
            UpdateButtons();

            if (_item is not UICard)
            {
                PrefixName.Editable = false;
                return;
            }

            byte calculatedLength = (byte)(16 - ((UICard)_item).CardName.Length);
            PrefixName.Editable = calculatedLength == 0 ? false : true;
            PrefixName.MaxLength = calculatedLength;
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
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.drawSounds.PickRandom());

        

        QueueFree();
        if (Item is UICard)
        {
            editor.gameMasterData.AddCardToPlayerCollection(((UICard)Item).CreateCardInstance());
        }
    }

    public void ButtonPressed(string option)
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.levelupSounds.PickRandom());


        QueueFree();
        this.option = option;
        Clicked.Invoke(this);
    }

    private void DeleteItem()
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.DeleteSounds.PickRandom());


        QueueFree();
        if (Item is UICard)
        {
            if (editor.CurrentMenu is PlayerCollection)
            {
                editor.gameMasterData.RemoveCardFromPlayerCollection(((UICard)Item).CreateCardInstance());
                return;
            }

            editor.gameMasterData.RemoveCardFromWorldCards(((UICard)Item).CreateCardInstance());

        }
        else if (Item is UIDungeon)
        {
            editor.gameMasterData.RemoveDungeonFromDungeonList(((UIDungeon)Item).CreateDungeonInstance());

        }
        else if (Item is UIBossCard)
        {
            editor.gameMasterData.RemoveCardFromWorldCards(((UIBossCard)Item).CreateBossCardInstance());

        }
    }

    private void UpdateButtons()
    {
        if (Item is UICard)
        {
            CollectionButton.Disabled = !Global.masterEditor.gameMasterData.TestCard(((UICard)Item).CreateCardInstance(), Global.masterEditor.gameMasterData.PlayerCollection);
            HPButton.Disabled = false;
            DMGButton.Disabled = false;
            PrefixName.Editable = true;
        }
        else if (Item is UIBossCard)
        {
            CollectionButton.Disabled = true;
            HPButton.Disabled = true;
            DMGButton.Disabled = true;
            PrefixName.Editable = false;
        }
        else if (Item is UIDungeon)
        {
            CollectionButton.Disabled = true;
            HPButton.Disabled = true;
            DMGButton.Disabled = true;
            PrefixName.Editable = false;
        }
    }

}
