using Godot;
using System;

public partial class DungeonStartButton : Button
{
    [Export] Label nameLabel;
    [Export] UIDungeon uIDungeon;

    public async override void _Pressed()
    {
        Global.gameManager.saverLoader.currSaveFile.currDungeonName = nameLabel.Text;
        await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.FightMap);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Global.gameManager.saverLoader.currSaveFile == null) return;
        if (Global.gameManager.saverLoader.currSaveFile.player.Deck.Count <= 0)
        {
            Disabled = true;
        }
        else
        {
            Disabled = false;
        }
        if (uIDungeon.DungeonType == DungeonTypes.big && !Utility.IsMoreCardsToGet(Global.gameManager.saverLoader.currSaveFile.WorldCards, Global.gameManager.saverLoader.currSaveFile.player.Collection))
        {
            Disabled = true;
        }
        
    }
}
