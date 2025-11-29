using Godot;
using System;

public partial class Inventory : PanelContainer
{

	[Export] PackedScene ItemSlotScene;
	[Export] GridContainer MainContainer;
	[Export] int AmountOfSlots = 4;

	public override void _Ready()
	{
		base._Ready();
		for (int i = 0; i < AmountOfSlots; i++)
		{
			ItemSlot itemSlot = ItemSlotScene.Instantiate<ItemSlot>();
			
			var child = itemSlot.GetChild<Control>(0);
			itemSlot.CustomMinimumSize = child.Size;
			
			if (i != 1)
				itemSlot.RemoveChild(child);
			
			MainContainer.AddChild(itemSlot);
		}
	}

	public override void _Process(double delta)
	{
		if (Input.GetCurrentCursorShape() == Input.CursorShape.Forbidden)
		{
			DisplayServer.CursorSetShape(DisplayServer.CursorShape.Arrow);
		}
	}

	private Control dataBk;
	public override void _Notification(int what)
	{
		if (what == Node.NotificationDragBegin)
		{
			dataBk = GetViewport().GuiGetDragData().As<Control>();
		}

		if (what == Node.NotificationDragEnd)
		{
			if (!IsDragSuccessful())
			{
				if (dataBk != null)
				{
					dataBk.Show();
					dataBk = null;
				}
			}
		}
	}

}
