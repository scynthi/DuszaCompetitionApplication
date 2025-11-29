using Godot;
using System;

public partial class BackButton : Button
{
    public async override void _Pressed()
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());

       	await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.DungeonMap);
    }

}
