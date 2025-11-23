using Godot;
using System;

public partial class ContinueGameSubMenu : Control
{
    [Export] PackedScene uiSaveFileContinueItem;
    [Export] Control saveFileContinueItemsHolder;

    public void ReloadSaves()
    {
        DirAccess dir = DirAccess.Open(SaveLoadSystem.SAVE_PATH);

        if (dir != null)
        {
            foreach (Node child in saveFileContinueItemsHolder.GetChildren())
            {
                child.QueueFree();
            }

            dir.ListDirBegin();
            string fileName = dir.GetNext();

            while (fileName != "")
            {

                if (fileName.EndsWith(".tres"))
                {
                    var saveFile = Global.gameManager.saverLoader.LoadSaveFile(fileName);

                    if (saveFile != null)
                    {
                        if (!saveFile.isStarted){
                            fileName = dir.GetNext();
                            continue;}

                        UiSaveFileContinueItem newUiSaveFileContinueItem = uiSaveFileContinueItem.Instantiate<UiSaveFileContinueItem>();

                        newUiSaveFileContinueItem.BindSaveFile(saveFile);
                        newUiSaveFileContinueItem.SaveOpened += _SaveFileContinueItemPressed;

                        saveFileContinueItemsHolder.AddChild(newUiSaveFileContinueItem);

                        newUiSaveFileContinueItem.SetLabelText(saveFile.name);

                    }
                }
                fileName = dir.GetNext();
            }
            dir.ListDirEnd();
        }
    }


    private async void _SaveFileContinueItemPressed(SaveFileResource bindedSaveFile)
    {
        Global.gameManager.saverLoader.currSaveFile = bindedSaveFile;
        await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.DungeonMap);
    }
        
}
