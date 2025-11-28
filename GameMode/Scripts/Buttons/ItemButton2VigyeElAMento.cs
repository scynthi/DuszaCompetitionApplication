using Godot;
using System;

public partial class ItemButton2VigyeElAMento : Button
{
	[Signal]
    public delegate void Send_ItemEventHandler(int item);

	[Export] public ItemType itemType;
    [Export] public DescriptionBox descriptionBox;
	private IItem item;

    public override void _Ready()
    {
        MouseEntered += Hover;
        MouseExited += NoHover;
		item = Items.CreateItemFromType(itemType);
        Icon = Utility.LoadTextureFromPath(item.Icon);
    }

	public override void _Pressed()
    {
        EmitSignal(SignalName.Send_Item, (int)itemType);
        descriptionBox.UpdateDescription(item.Name, item.Description, Global.gameManager.saverLoader.currSaveFile.player.ReturnItemAmount(item));
    }

	private void Hover()
    {   
        descriptionBox.ShowDescription(item.Name, item.Description, Global.gameManager.saverLoader.currSaveFile.player.ReturnItemAmount(item), GlobalPosition + new Vector2(0, -5), Size.X);
    }
	private void NoHover()
    {
        descriptionBox.HideDescription();
    }
	
}
