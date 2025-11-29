using Godot;
using System;

public partial class UiSaveFileContinueItem : Control
{
    [Signal] public delegate void SaveOpenedEventHandler(WorldContext saveFileResource);

    private Button button;
    private Label label;


    public WorldContext bindedSaveFile { get; private set; }

    public override void _Ready()
    {
        button = GetNode<Button>("Button");
        label = GetNode<Label>("Label");
    }


    public void BindSaveFile(WorldContext saveFile)
    {
        bindedSaveFile = saveFile;
    }

    public void SetLabelText(string newText)
    {
        label.Text = newText;
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

    public void DeletButtonPressed()
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());
        
        SaverLoader.DeleteSave(bindedSaveFile.Name);
        this.QueueFree();
    }
}
