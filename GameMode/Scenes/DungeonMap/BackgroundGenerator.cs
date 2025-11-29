using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class BackgroundGenerator : Node2D
{
    [Export] public PackedScene mapBackground;
    [Export] public PackedScene mapBackgroundV2;
    [Export] public PackedScene mapBackgroundFinal;

    [Export] public int backgroundWidth = 640;
    [Export] public int dungeonsPerTile = 6;
    [Export] public PackedScene dungeonButton;

    [Export] public Label tipLabel;

    public override void _Ready()
    {
        tipLabel.Visible = false;

        for (int i = 0; i < Math.Ceiling((float)Global.gameManager.saverLoader.currSaveFile.WorldDungeons.Count / (float)dungeonsPerTile); i++)
        {
            Node2D newBackgroundTile = null;

            if (i + 1 == Math.Ceiling((float)Global.gameManager.saverLoader.currSaveFile.WorldDungeons.Count / (float)dungeonsPerTile))
            {
                newBackgroundTile = mapBackgroundFinal.Instantiate<Node2D>();
            }
            else
            {
                tipLabel.Visible = true;
                switch (Math.Round(GD.Randf()))
                {
                    case 0:
                        newBackgroundTile = mapBackground.Instantiate<Node2D>();
                        break;
                    case 1:
                        newBackgroundTile = mapBackgroundV2.Instantiate<Node2D>();
                        break;
                }
            }

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

        foreach(Dungeon dungeon in Global.gameManager.saverLoader.currSaveFile.WorldDungeons)
        {
            MapDungeonButton newMapDungeonButton = (MapDungeonButton)dungeonButton.Instantiate();

            markers[dungeonIndex].AddChild(newMapDungeonButton);

            newMapDungeonButton.uIDungeon.SetUpDungeon(dungeon);
            newMapDungeonButton.uIDungeon.PreviewMode = false;

            dungeonIndex++;
        }
    }

}
