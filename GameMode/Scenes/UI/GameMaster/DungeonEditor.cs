using Godot;
using System;
using System.Linq;

public partial class DungeonEditor : HBoxContainer
{
    [Export] UIDungeon dungeon;
    [Export] OptionButton rewardType;
    [Export] Inventory dungeonDeck;
    [Export] Inventory worldCards;

    Editors editor = Global.masterEditor;

    public override void _Ready()
    {
        base._Ready();
        dungeonDeck.RemakePanelItems(1);
        dungeonDeck.IsBossCardNeeded = false;
        worldCards.RemakePanelItems(20);
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
        } else if (rewardType.Disabled)
        {
            rewardType.RemoveItem(2);
            rewardType.Selected = 0;
            rewardType.Disabled = false;
        }

        switch (type)
        {
            case DungeonTypes.simple:
                dungeonDeck.RemakePanelItems(1);
                dungeonDeck.IsBossCardNeeded = false;
                break;
            case DungeonTypes.small:
                dungeonDeck.RemakePanelItems(4, true);
                dungeonDeck.IsBossCardNeeded = true;
                break;
            case DungeonTypes.big:
                dungeonDeck.RemakePanelItems(6);
                dungeonDeck.IsBossCardNeeded = true;
                break;
        }
    }

    public void ChangeReward(int index)
    {
        dungeon.EditReward((DungeonRewardTypes)index);
    }
    
    public void SaveDungeon()
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());

        Dungeon dungeonInstance = dungeon.CreateDungeonInstance();
        if (!editor.gameMasterData.TesdtDungeon(dungeonInstance)) return;
        editor.gameMasterData.AddDungeonToDungeonList(dungeonInstance);
    }
}
