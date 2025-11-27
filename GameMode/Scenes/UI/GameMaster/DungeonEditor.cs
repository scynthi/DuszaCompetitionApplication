using Godot;
using System;
using System.Linq;

public partial class DungeonEditor : HBoxContainer
{
    [Export] UIDungeon dungeon;
    [Export] OptionButton rewardType;

    Editors editor = Global.masterEditor;

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
    }

    public void ChangeReward(int index)
    {
        dungeon.EditReward((DungeonRewardTypes)index);
    }
    
    // TODO: rewrite it when backend arrives
    // TODO: Do more checks for name
    public void SaveDungeon()
    {
        editor.gameMasterData.AddDungeonToDungeonList(dungeon.CreateDungeonInstance());
    }
}
