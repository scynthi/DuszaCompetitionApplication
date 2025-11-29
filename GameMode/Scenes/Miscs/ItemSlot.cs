using Godot;
using System;

public partial class ItemSlot : PanelContainer
{
	[Export] public TextureRect card;

	public override Variant _GetDragData(Vector2 atPosition)
	{
		if (card == null)
			return default;

		// Create a preview of just the card, not the whole slot
		var preview = new TextureRect();
		preview.Texture = card.Texture;
		preview.CustomMinimumSize = card.Size;
		preview.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
		preview.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
		
		Control c = new Control();
		c.AddChild(preview);
		c.Modulate = new Color(1, 1, 1, 0.5f);
		SetDragPreview(c);
		
		card.Hide();
		return card; // Return the child card, not the slot
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		return data.As<TextureRect>() != null;
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		TextureRect droppedCard = data.As<TextureRect>();
		if (droppedCard == null)
			return;

		// Get the old parent slot
		ItemSlot oldSlot = droppedCard.GetParent() as ItemSlot;
		if (oldSlot == null)
			return;

		// Swap cards between slots
		if (card != null)
		{
			GD.Print("BRUH2");
			// This slot has a card - swap them
			oldSlot.RemoveChild(oldSlot.GetChild(0));
			card.Reparent(oldSlot);
			oldSlot.card = card;
			card.Show();
		}
		else
		{
			GD.Print("BRUH");
			// This slot is empty - just clear old slot
			oldSlot.RemoveChild(oldSlot.GetChild(0));
			oldSlot.card = null;
		}

		// Reparent the dropped card to this slot
		droppedCard.Owner = null;
		AddChild(droppedCard);
		card = droppedCard;
		
		card.Show();
	}
}