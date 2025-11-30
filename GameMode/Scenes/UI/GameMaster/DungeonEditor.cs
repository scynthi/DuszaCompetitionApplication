using Godot;
using System;
using System.Linq;

public partial class DungeonEditor : HBoxContainer
{
    [Export] UIDungeon dungeon;
    [Export] OptionButton rewardType;
    [Export] Button saveButton;
    [Export] Inventory dungeonDeck;
    [Export] Inventory worldCards;

    Editors editor = Global.masterEditor;

    public override void _Ready()
    {
        base._Ready();
        dungeonDeck.IsDungeonDeck = true;
        dungeonDeck.RemakePanelItems(1);
        VisibilityChanged += OnVisibilityChanged;
        saveButton.Disabled = true;
        dungeonDeck.DeckIsFull += DeckIsFull;
        dungeonDeck.DeckIsNotFull += DeckIsNotFull;
        editor.gameMasterData.CardDataChanged += OnVisibilityChanged;
    }

    public void OnVisibilityChanged()
    {
        worldCards.RemakePanelItems(Collection: editor.gameMasterData.WorldCards);
        dungeonDeck.ClearCards();
        DeckIsNotFull();
    }

    public void DeckIsFull()
    {
        saveButton.Disabled = false;
    }

    public void DeckIsNotFull()
    {
        saveButton.Disabled = true;
    }

    public void ChangeName(string text)
    {
        dungeon.EditName(text);
    }

    public void ChangeType(int index)
    {
        DungeonTypes type = (DungeonTypes)index;
        dungeon.EditType(type);

        if (type == DungeonTypes.big)
        {
            rewardType.AddItem("kártya", 2);
            rewardType.Selected = (int)DungeonRewardTypes.card;
            rewardType.Disabled = true;
            ChangeReward(rewardType.Selected);
        } else if (rewardType.Disabled)
        {
            rewardType.RemoveItem(2);
            rewardType.Selected = 0;
            rewardType.Disabled = false;
            ChangeReward(rewardType.Selected);
        }

        switch (type)
        {
            case DungeonTypes.simple:
                dungeonDeck.RemakePanelItems(1);
                break;
            case DungeonTypes.small:
                dungeonDeck.RemakePanelItems(4, true, IsBossCardNeeded: true);
                break;
            case DungeonTypes.big:
                dungeonDeck.RemakePanelItems(6, IsBossCardNeeded: true);
                break;
        }
        OnVisibilityChanged();
        saveButton.Disabled = true;
    }

    public void ChangeReward(int index)
    {
        dungeon.EditReward((DungeonRewardTypes)index);
    }
    
    public void SaveDungeon()
    {
        Dungeon dungeonInstance = dungeon.CreateDungeonInstance(dungeonDeck.ReturnContents());

        if (!editor.gameMasterData.TestDungeon(dungeonInstance)) return;
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.levelupSounds.PickRandom());

        editor.gameMasterData.AddDungeonToDungeonList(dungeonInstance);
    }
}
