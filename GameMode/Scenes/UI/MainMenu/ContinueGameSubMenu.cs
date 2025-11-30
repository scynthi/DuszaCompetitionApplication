using Godot;
using System;

public partial class ContinueGameSubMenu : Control
{
    [Export] PackedScene uiSaveFileContinueItem;
    [Export] Control saveFileContinueItemsHolder;

    public void ReloadSaves()
    {
        DirAccess dir = DirAccess.Open(SaverLoader.CONTINUE_PATH);

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

                if (!fileName.Contains("."))
                {
                    var saveFile = Global.gameManager.saverLoader.Load(fileName, SaverLoader.CONTINUE_PATH);

                    if (saveFile != null)
                    {
                        UiSaveFileContinueItem newUiSaveFileContinueItem = uiSaveFileContinueItem.Instantiate<UiSaveFileContinueItem>();

                        newUiSaveFileContinueItem.BindSaveFile(saveFile);
                        newUiSaveFileContinueItem.SaveOpened += _SaveFileContinueItemPressed;

                        saveFileContinueItemsHolder.AddChild(newUiSaveFileContinueItem);

                        newUiSaveFileContinueItem.SetLabelText(saveFile.Name);

                    }
                }
                fileName = dir.GetNext();
            }
            dir.ListDirEnd();
        }
    }


    private async void _SaveFileContinueItemPressed(WorldContext bindedSaveFile)
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());

        Global.gameManager.saverLoader.currSaveFile = bindedSaveFile;
        await Global.gameManager.ChangeWorldScene(GameManager.ScenePaths.DungeonMap);
    }
        
}
