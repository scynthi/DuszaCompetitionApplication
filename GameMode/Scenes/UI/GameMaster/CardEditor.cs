using Godot;
using System.Linq;
using System.Numerics;

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
                card.EditIcon(Global.gameManager.uiPackedSceneReferences.ManTexture);
                break;
            case 1:
                card.EditIcon(Global.gameManager.uiPackedSceneReferences.WomanTexture);
                break;
            case 2:
                card.EditIcon(Global.gameManager.uiPackedSceneReferences.GoblinTexture);
                break;
            case 3:
                card.EditIcon(Global.gameManager.uiPackedSceneReferences.WendigoTexture);
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
                // fileDialogInstance.Size = new Godot.Vector2I(1000, 570);

                GetTree().CurrentScene.AddChild(fileDialogInstance);
                break;
        }
    }

    private void HandleFile(object receivedInfo)
    {
        card.EditIcon((string)receivedInfo);
    }

    public void SaveCard()
    {
        if (!Global.masterEditor.gameMasterData.TestCard(card.CreateCardInstance())) return;

        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.levelupSounds.PickRandom());

        editor.gameMasterData.AddCardToWorldCards(card.CreateCardInstance());
    }
}
