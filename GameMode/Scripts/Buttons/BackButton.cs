using Godot;
using System;

public partial class BackButton : Button
{
    public async override void _Pressed()
    {
       	await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.DungeonMap);
    }

}
