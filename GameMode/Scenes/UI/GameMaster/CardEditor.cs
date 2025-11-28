using Godot;
using System;
using System.Linq;
using System.Reflection.Metadata;

public partial class CardEditor : HBoxContainer
{
	[Export] UICard card;

    Editors editor = Global.masterEditor;

	public void ChangeName(string text)
    {
        card.EditName(text);
    }

    public void ChangeHealth(float number)
    {   
        card.EditHealth((int)number);
    }

    public void ChangeDamage(float number)
    {
        card.EditDamage((int)number);
    }

    public void ChangeElement(int index)
    {
        card.EditElement((CardElements)index);
    }

    public void ChangeIcon(int index)
    {
        switch(index)
        {
            case 0:
                card.EditIcon(Global.gameManager.uiPackedSceneReferences.ManTexture.ResourcePath);
                break;
            case 1:
                card.EditIcon(Global.gameManager.uiPackedSceneReferences.WomanTexture.ResourcePath);
                break;
            case 2:
                card.EditIcon(Global.gameManager.uiPackedSceneReferences.GoblinTexture.ResourcePath);
                break;
            case 3:
                card.EditIcon(Global.gameManager.uiPackedSceneReferences.WendigoTexture.ResourcePath);
                break;
            case 4:
                FileDialog fileDialogInstance = new();
                fileDialogInstance.Access = FileDialog.AccessEnum.Filesystem;
                fileDialogInstance.FileMode = FileDialog.FileModeEnum.OpenFile;
                fileDialogInstance.InitialPosition = Window.WindowInitialPosition.CenterMainWindowScreen;
                fileDialogInstance.FileSelected += HandleFile;
                fileDialogInstance.CurrentDir = "C:/";
                fileDialogInstance.AddFilter("*.png");
                fileDialogInstance.Visible = true;

                GetTree().CurrentScene.AddChild(fileDialogInstance);
                break;
        }
    }

    private void HandleFile(object receivedInfo)
    {
        card.EditIcon((string)receivedInfo);
    }

    // TODO: Do more checks for name
    public void SaveCard()
    {
        editor.gameMasterData.AddCardToWorldCards(card.CreateCardInstance());
    }

}
