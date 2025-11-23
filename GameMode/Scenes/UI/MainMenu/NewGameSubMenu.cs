using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

public partial class NewGameSubMenu : Control
{
    [Export] PackedScene uiSaveFileItem;
    [Export] Control saveFileItemsHolder;

    public void ReloadSaves()
    {
        DirAccess dir = DirAccess.Open(SaveLoadSystem.SAVE_PATH);

        if (dir != null)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();

            while (fileName != "")
            {
                if (fileName.EndsWith(".tres"))
                {
                    var saveFile = Global.gameManager.saverLoader.LoadSaveFile(fileName);
                    
                    if (saveFile != null)
                    {
                        UiSaveFileItem newUiSaveFileItem = (UiSaveFileItem)uiSaveFileItem.Instantiate();

                        newUiSaveFileItem.BindSaveFile(saveFile);
                        newUiSaveFileItem.SaveOpened += _SaveFileItemPressed;

                        saveFileItemsHolder.AddChild(newUiSaveFileItem);

                        newUiSaveFileItem.SetButtonText(saveFile.name);

                    }
                }
                fileName = dir.GetNext();
            }
            dir.ListDirEnd();
        }
    }

    private void _SaveFileItemPressed(SaveFileResource bindedSaveFile)
    {
        GD.Print(bindedSaveFile.name);
    }

}
