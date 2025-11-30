using Godot;
using System;

public partial class ItemSlot : PanelContainer
{
	[Signal]
	public delegate void NewCardAddedEventHandler();
	[Signal]
	public delegate void CardTakenOutEventHandler();
	PackedScene UICard;
	[Export] public Control uiCard;
	public bool IsBossCardSlot = false;

	public override void _Ready()
	{
		UICard = GD.Load<PackedScene>("uid://dk32ss75ce3lw");
		// Make sure mouse events reach the ItemSlot, not the child
		if (uiCard != null)
		{
			SetMouseFilterRecursive(uiCard);
		}
	}

	void SetMouseFilterRecursive(Node node)
	{
		if (node is Control control)
		{
			control.MouseFilter = Control.MouseFilterEnum.Ignore;
		}
		
		foreach (Node child in node.GetChildren())
		{
			SetMouseFilterRecursive(child);
		}
	}

	public override Variant _GetDragData(Vector2 atPosition)
	{
		if (uiCard == null)
			return default;

		// Create a preview and copy the card data
		Control preview = Duplicate() as Control;
		Control c = new Control();
		c.AddChild(preview);

		preview.Position = -uiCard.Size / 2;
		preview.ZIndex = 100;
		c.Modulate = new Color(1, 1, 1, 0.5f);
		SetDragPreview(c);
		
		uiCard.Hide();
		return uiCard; // Return the child uiCard, not the slot
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		return data.As<Control>() != null;
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		Control droppedCard = data.As<Control>();
		if (droppedCard == null)
			return;

		if (droppedCard is not UIBossCard && IsBossCardSlot)
			return;

		// Get the old parent slot
		ItemSlot oldSlot = droppedCard.GetParent() as ItemSlot;
		if (oldSlot == null)
			return;

		if (oldSlot == this)
		{
			droppedCard.Show();
			return;
		}

		// Swap cards between slots
		if (uiCard != null)
		{
			// This slot has a uiCard - swap them
			oldSlot.RemoveChild(oldSlot.GetChild(0));
			uiCard.Reparent(oldSlot);
			oldSlot.uiCard = uiCard;
			uiCard.Show();
		}
		else
		{
			// This slot is empty - just clear old slot
			oldSlot.RemoveChild(oldSlot.GetChild(0));
			oldSlot.uiCard = null;
		}

		// Reparent the dropped uiCard to this slot
		droppedCard.Owner = null;
		AddChild(droppedCard);
		uiCard = droppedCard;
		
		uiCard.Show();
		oldSlot.EmitSignal(SignalName.CardTakenOut);
		EmitSignal(SignalName.NewCardAdded);
	}
}