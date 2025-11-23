using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class BackgroundGenerator : Node2D
{
    [Export] public PackedScene mapBackground;
    [Export] public int backgroundWidth = 640;
    [Export] public int dungeonsPerTile = 6;
    [Export] public PackedScene dungeonButton;

    public override void _Ready()
    {
        for (int i = 0; i < Math.Ceiling((float)Global.gameManager.saverLoader.currSaveFile.dungeons.Count / (float)dungeonsPerTile); i++)
        {
            Node2D newBackgroundTile = (Node2D)mapBackground.Instantiate();

            this.AddChild(newBackgroundTile);
            newBackgroundTile.Position = new Vector2(i * backgroundWidth, 0.0f);
        }

        LoadDungeonInstances();   
    }

    private void LoadDungeonInstances()
    {
        List<Marker2D> markers = GetTree()
            .GetNodesInGroup("DungeonPositions")
            .OfType<Marker2D>()
            .ToList();
        
        int dungeonIndex = 0;

        foreach(Dungeon dungeon in Global.gameManager.saverLoader.currSaveFile.dungeons)
        {
            MapDungeonButton newMapDungeonButton = (MapDungeonButton)dungeonButton.Instantiate();

            markers[dungeonIndex].AddChild(newMapDungeonButton);
            
            GD.Print(dungeon.Name);
            newMapDungeonButton.uIDungeon.SetUpDungeon(dungeon);

            dungeonIndex++;
        }
    }

}
