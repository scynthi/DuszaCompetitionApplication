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
    [Export] RichTextLabel displayCardAmount;
    [Export] Godot.Collections.Array<Label> displayCardElements;
    [Export] RichTextLabel displayDungeonAmount;
    [Export] Godot.Collections.Array<Label> displayDungeonIcons;


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

    private void _SaveFileItemPressed(WorldContext      bindedSaveFile)
    {
        inspectorField.Visible = true;

        currDisplayedSave = bindedSaveFile;

        displaySaveName.Text = bindedSaveFile.Name;

        List<Card> worldCardList = bindedSaveFile.WorldCards.ToList();
        List<Dungeon> dungeonList = bindedSaveFile.WorldDungeons.ToList();

        int fireCount = worldCardList.Count(c => c.CardElement == CardElements.FIRE);
        int waterCount = worldCardList.Count(c => c.CardElement == CardElements.WATER);
        int earthCount = worldCardList.Count(c => c.CardElement == CardElements.EARTH);
        int windCount = worldCardList.Count(c => c.CardElement == CardElements.WIND);

        displayCardElements[0].Text = $" {fireCount}x";
        displayCardElements[1].Text = $" {waterCount}x";
        displayCardElements[2].Text = $" {earthCount}x";
        displayCardElements[3].Text = $" {windCount}x";
        displayCardAmount.Text = "Összesen: "+$"{bindedSaveFile.WorldCards.Count}";

        int simpleCount = dungeonList.Count(c => c.DungeonType == DungeonTypes.simple);
        int smallCount = dungeonList.Count(c => c.DungeonType == DungeonTypes.small);
        int bigCount = dungeonList.Count(c => c.DungeonType == DungeonTypes.big);

        displayDungeonIcons[0].Text = $" {simpleCount}x";
        displayDungeonIcons[1].Text = $" {smallCount}x";
        displayDungeonIcons[2].Text = $" {bigCount}x";
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
