using Godot;
using System;

public partial class UiSaveFileContinueItem : Control
{
    [Signal] public delegate void SaveOpenedEventHandler(SaveFileResource saveFileResource);

    private Button button;
    private Label label;


    public SaveFileResource bindedSaveFile {get; private set;}

    public override void _Ready()
    {
        button = GetNode<Button>("Button");
        label = GetNode<Label>("Label");
    }


    public void BindSaveFile(SaveFileResource saveFile)
    {
        bindedSaveFile = saveFile;
    }

    public void SetLabelText(string newText)
    {
        label.Text = newText;
    }

    public void _Pressed()
    {
        EmitSignal(SignalName.SaveOpened, bindedSaveFile);
    }
}
