using Godot;
using System;

public partial class ItemButton : Button
{
	[Signal]
    public delegate void Send_ItemEventHandler(int item);

	[Export] private ItemType itemType;
	private IItem item;

    public override void _Ready()
    {
        MouseEntered += Hover;
        MouseExited += NoHover;
		item = Items.CreateItemFromType(itemType);
    }


	public override void _Pressed()
    {
        EmitSignal(SignalName.Send_Item, (int)itemType);
    }

	private void Hover()
    {
        DescriptionBox.Instance.ShowDescription(item.Name, item.Description, GetViewport().GetMousePosition());
    }
	private void NoHover()
    {
        DescriptionBox.Instance.HideDescription();
    }
	
}
