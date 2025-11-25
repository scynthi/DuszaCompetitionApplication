using Godot;
using System;
using System.Linq;
using System.Reflection.Metadata;

public partial class CardEditor : HBoxContainer
{
	[Export] UICard card;

    Editors editor;

    //If you dont understand why is this here, ask Dani. -Dani "Hey, yep, heyo, this was me, Dani" -Dani
	public void HandleDataChange() {}

    public override void _Ready()
    {
        editor = (Editors)GetParent();
    }

	public void ChangeName(string text)
    {
        card.EditName(text);
    }

    public void ChangeHealth(string text)
    {
        if (!int.TryParse(text, out _) || text.Replace(" ", "") == "") return;
        
        card.EditHealth(int.Parse(text));
    }

    public void ChangeDamage(string text)
    {
        if (!int.TryParse(text, out _) || text.Replace(" ", "") == "") return;

        card.EditDamage(int.Parse(text));
    }

    public void ChangeElement(int index)
    {
        card.EditElement((CardElements)index);
    }

    public void ChangeIcon()
    {
        FileDialog fileDialogInstance = new();
        fileDialogInstance.Access = FileDialog.AccessEnum.Filesystem;
        fileDialogInstance.FileMode = FileDialog.FileModeEnum.OpenFile;
        fileDialogInstance.InitialPosition = Window.WindowInitialPosition.CenterMainWindowScreen;
        fileDialogInstance.FileSelected += HandleFile;
        fileDialogInstance.CurrentDir = "C:/";
        fileDialogInstance.AddFilter("*.png");
        fileDialogInstance.Visible = true;

        GetTree().CurrentScene.AddChild(fileDialogInstance);
    }

    private void HandleFile(object receivedInfo)
    {
        FileAccess file = FileAccess.Open(receivedInfo.ToString(), FileAccess.ModeFlags.Read);
        byte[] buffer = file.GetBuffer((long)file.GetLength());
        Image image = new Image();
        Error err = image.LoadPngFromBuffer(buffer);

        if (err != Error.Ok) GD.PrintErr($"Failed to load image! {receivedInfo}");

        card.EditIcon(image);
    }

    // TODO: Do more checks for name

    public void SaveCard()
    {
        editor.gameMasterData.AddCardToWorldCards(card.CreateCardInstance());
    }

}
