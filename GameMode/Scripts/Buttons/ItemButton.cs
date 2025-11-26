using Godot;
using System;

public partial class ItemButton : Button
{
	[Signal]
    public delegate void Send_ItemEventHandler(int item);
	[Signal]
    public delegate void Send_Item_RemovedEventHandler(int item);
    [Signal]
    public delegate void Send_Item_AddedEventHandler(int item);

	[Export] public ItemType itemType;
    [Export] public DescriptionBox descriptionBox;
    [Export] public Label priceLabel;
	private IItem item;

    public override void _Ready()
    {
        MouseEntered += Hover;
        MouseExited += NoHover;
		item = Items.CreateItemFromType(itemType);
        Icon = Utility.LoadTextureFromPath(item.Icon);
        priceLabel.Text = "$" + Convert.ToString(item.Price);
    }

	public override void _Pressed()
    {
        EmitSignal(SignalName.Send_Item, (int)itemType);
        descriptionBox.UpdateDescription(item.Name, item.Description, Global.gameManager.saverLoader.currSaveFile.player.ReturnItemAmount(item));
    }

    public override void _Toggled(bool toggledOn)
    {
        base._Toggled(toggledOn);
        if (toggledOn)
        {
            EmitSignal(SignalName.Send_Item_Added, (int)itemType);
            return;
        }
        EmitSignal(SignalName.Send_Item_Removed, (int)itemType);
    }


	private void Hover()
    {   
        descriptionBox.ShowDescription(item.Name, item.Description, Global.gameManager.saverLoader.currSaveFile.player.ReturnItemAmount(item), GlobalPosition + new Vector2(0, -5), Size.X);
    }
	private void NoHover()
    {
        descriptionBox.HideDescription();
    }
    public void SetToggledOn()
    {
        ToggleMode = true;
    }
	
}
