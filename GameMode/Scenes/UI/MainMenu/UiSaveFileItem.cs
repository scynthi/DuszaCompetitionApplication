using Godot;
using System;

public partial class UiSaveFileItem : Control
{
    [Signal] public delegate void SaveOpenedEventHandler(SaveFileResource saveFileResource);

    private Button button;

    public SaveFileResource bindedSaveFile {get; private set;}

    public override void _Ready()
    {
        button = GetNode<Button>("Button");
    }


    public void BindSaveFile(SaveFileResource saveFile)
    {
        bindedSaveFile = saveFile;
    }

    public void SetButtonText(string newText)
    {
        button.Text = newText;
    }

    public void _Pressed()
    {
        EmitSignal(SignalName.SaveOpened, bindedSaveFile);
    }
    
}
