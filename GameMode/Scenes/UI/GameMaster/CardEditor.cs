using Godot;
using System;
using System.Reflection.Metadata;

public partial class CardEditor : HBoxContainer
{
	[Export] UICard card;

	public void ChangeName(string text)
    {
        card.EditName(text);
    }

    public void ChangeHealth(string text)
    {
        if (!int.TryParse(text, out _) || text.Replace(" ", "") == "") return;
        
        card.EditHealth(Math.Clamp(int.Parse(text), 1, 100));
    }

    public void ChangeDamage(string text)
    {
        if (!int.TryParse(text, out _) || text.Replace(" ", "") == "") return;

        card.EditDamage(Math.Clamp(int.Parse(text), 1, 100));
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
        fileDialogInstance.AddFilter("*.png");

        GetTree().CurrentScene.AddChild(fileDialogInstance);
        fileDialogInstance.Visible = true;

        fileDialogInstance.FileSelected += HandleFile;
    }

    private void HandleFile(object sender)
    {
        FileAccess file = FileAccess.Open(sender.ToString(), FileAccess.ModeFlags.Read);
        byte[] buffer = file.GetBuffer((long)file.GetLength());
        // Image image = new Image().LoadPngFromBuffer(buffer);
    }

    // public void ChangeType()
    // {
    //     card.isBoss = !card.isBoss;
    //     card.EditEffect();
    // }

    public void SaveCard()
    {
        GD.Print($"{card.CardName} {card.CardDamage}, {card.CardHealth}, {card.CardElement}, {card.isBoss}");
    }

}
