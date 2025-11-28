using Godot;
using System;

public partial class DungeonStartButton : Button
{
    [Export] Label nameLabel;

    public async override void _Pressed()
    {
        Global.gameManager.saverLoader.currSaveFile.currDungeonName = nameLabel.Text;
        await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.FightMap);
    }

}
