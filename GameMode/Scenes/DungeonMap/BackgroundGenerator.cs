using Godot;
using System;

public partial class BackgroundGenerator : Node2D
{
    [Export] public PackedScene mapBackground;
    [Export] public int backgroundWidth = 640;
    [Export] public int dungeonsPerTile = 6;

    public override void _Ready()
    {
        for (int i = 0; i < Math.Ceiling((float)Global.gameManager.saverLoader.currSaveFile.dungeons.Count / (float)dungeonsPerTile); i++)
        {
            Node2D newBackgroundTile = (Node2D)mapBackground.Instantiate();

            this.AddChild(newBackgroundTile);
            newBackgroundTile.Position = new Vector2(i * backgroundWidth, 0.0f);
        }
    }


}
