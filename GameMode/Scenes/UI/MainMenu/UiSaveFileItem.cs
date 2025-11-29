using Godot;
using System;

public partial class UiSaveFileItem : Control
{
    [Signal] public delegate void SaveOpenedEventHandler(WorldContext saveFileResource);

    private Button button;

    public WorldContext bindedSaveFile {get; private set;}

    public override void _Ready()
    {
        button = GetNode<Button>("Button");
    }


    public void BindSaveFile(WorldContext saveFile)
    {
        bindedSaveFile = saveFile;
    }

    public void SetButtonText(string newText)
    {
        button.Text = newText;

        if (bindedSaveFile.WorldDungeons.Count <= 0)
        {
            button.Disabled = true;
        }
    }

    public void _Pressed()
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());

        EmitSignal(SignalName.SaveOpened, bindedSaveFile);
    }
    
}
