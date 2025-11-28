using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;

public partial class NewGameSubMenu : Control
{
    [Export] PackedScene uiSaveFileItem;
    [Export] Control saveFileItemsHolder;

    [ExportGroup("InspectorField")]
    [Export] Control inspectorField;
    [Export] Label displaySaveName;
    [Export] RichTextLabel displayCardElements;
    [Export] RichTextLabel displayCardAmount;
    [Export] RichTextLabel displayDungeonElements;
    [Export] RichTextLabel displayDungeonAmount;

    private WorldContext currDisplayedSave; 

    public void ReloadSaves()
    {
        DirAccess dir = DirAccess.Open(SaverLoader.SAVE_PATH);

        if (dir != null)
        {
            foreach (Node child in saveFileItemsHolder.GetChildren())
            {
                child.QueueFree();
            }

            dir.ListDirBegin();
            string fileName = dir.GetNext();

            while (fileName != "")
            {
                if (!fileName.Contains("."))
                {
                    var saveFile = Global.gameManager.saverLoader.Load(fileName, SaverLoader.SAVE_PATH);
                    
                    if (saveFile != null)
                    {
                        UiSaveFileItem newUiSaveFileItem = (UiSaveFileItem)uiSaveFileItem.Instantiate();

                        newUiSaveFileItem.BindSaveFile(saveFile);
                        newUiSaveFileItem.SaveOpened += _SaveFileItemPressed;

                        saveFileItemsHolder.AddChild(newUiSaveFileItem);

                        newUiSaveFileItem.SetButtonText(saveFile.Name);

                    }
                }
                fileName = dir.GetNext();
            }
            dir.ListDirEnd();
        }

        inspectorField.Visible = false;
    }

    private void _SaveFileItemPressed(WorldContext bindedSaveFile)
    {
        inspectorField.Visible = true;

        currDisplayedSave = bindedSaveFile;

        displaySaveName.Text = bindedSaveFile.Name;

        int earthCount = bindedSaveFile.WorldCards.ToList<Card>().Count(c => c.CardElement == CardElements.EARTH);
        int windCount = bindedSaveFile.WorldCards.ToList<Card>().Count(c => c.CardElement == CardElements.WIND);
        int waterCount = bindedSaveFile.WorldCards.ToList<Card>().Count(c => c.CardElement == CardElements.WATER);
        int fireCount = bindedSaveFile.WorldCards.ToList<Card>().Count(c => c.CardElement == CardElements.FIRE);

        string cardText = $"{fireCount}x-[color=red]Tűz[/color] {waterCount}x-[color=blue]Víz[/color] {earthCount}x-[color=green]Föld[/color] {windCount}x-[color=cyan]Szél[/color]";
        displayCardElements.Text = cardText;
        displayCardAmount.Text = "Összesen: "+$"{bindedSaveFile.WorldCards.Count}";


        int simpleCount = bindedSaveFile.WorldDungeons.ToList<Dungeon>().Count(c => c.DungeonType == DungeonTypes.simple);
        int smallCount = bindedSaveFile.WorldDungeons.ToList<Dungeon>().Count(c => c.DungeonType == DungeonTypes.small);
        int bigCount = bindedSaveFile.WorldDungeons.ToList<Dungeon>().Count(c => c.DungeonType == DungeonTypes.big);

        string dungeonText = $"{simpleCount}x-[color=gray]Sima[/color] {smallCount}x-[color=blue]Kis[/color] {bigCount}x-[color=red]Nagy[/color]";
        displayDungeonElements.Text = dungeonText;
        displayDungeonAmount.Text = "Összesen: "+$"{bindedSaveFile.WorldDungeons.Count}";
    }

    public void _OnDifficultyValueChanged(float value)
    {
        currDisplayedSave.gameDifficulty = (int)Math.Round(value);
    }

    public async void _OnStartPressed()
    {
        Global.gameManager.saverLoader.currSaveFile = currDisplayedSave;
        await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.DungeonMap);
    }
}
