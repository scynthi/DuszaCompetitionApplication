using Godot;
using System;

public partial class ItemSlot : Panel
{
	[Signal]
	public delegate void NewCardAddedEventHandler();
	[Signal]
	public delegate void CardTakenOutEventHandler();
	PackedScene UICard;
	[Export] public Control uiCard;
	public bool IsBossCardSlot = false;
	private bool _dragStarted = false;
	private Tween _hoverTween;
	private Vector2 _originalPosition;
	private Vector2 _originalScale;
	private float _hoverOffset = -10f;
	private Vector2 _hoverScaleMultiplier = new Vector2(1.1f, 1.1f);
	public Inventory ParentInventory { get; set; }

	public override void _Ready()
	{
		UICard = GD.Load<PackedScene>("uid://dk32ss75ce3lw");
		// Make sure mouse events reach the ItemSlot, not the child
		if (uiCard != null)
		{
			SetMouseFilterRecursive(uiCard);
			Vector2 slotSize = Size;
			Vector2 cardSize = uiCard.Size;
			float scaleX = slotSize.X / cardSize.X;
			float scaleY = slotSize.Y / cardSize.Y;
			float scale = Mathf.Min(scaleX, scaleY);

			uiCard.Scale = new Vector2(scale, scale);
		}

		MouseEntered += OnMouseEntered;
    	MouseExited += OnMouseExited;
	}

	public void OnMouseEntered()
	{
		// if (uiCard == null || _dragStarted)
		// 	return;
		
		// _hoverTween?.Kill();
		// _hoverTween = CreateTween();
		// _hoverTween.SetParallel(true);
		// _hoverTween.SetEase(Tween.EaseType.Out);
		// _hoverTween.SetTrans(Tween.TransitionType.Back);
		
		// _hoverTween.TweenProperty(uiCard, "position:y", _originalPosition.Y + _hoverOffset, 0.3);
		// _hoverTween.TweenProperty(uiCard, "scale", _originalScale * _hoverScaleMultiplier, 0.3);
	}

	public void OnMouseExited()
	{
		// if (uiCard == null)
		// 	return;
		
		// _hoverTween?.Kill();
		// _hoverTween = CreateTween();
		// _hoverTween.SetParallel(true);
		// _hoverTween.SetEase(Tween.EaseType.Out);
		// _hoverTween.SetTrans(Tween.TransitionType.Cubic);
		
		// _hoverTween.TweenProperty(uiCard, "position", _originalPosition, 0.2);
		// _hoverTween.TweenProperty(uiCard, "scale", _originalScale, 0.2);
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
		_dragStarted = true;

		if (uiCard == null)
			return default;


		// Create a preview and copy the card data
		Control preview = Duplicate() as Control;
		Control c = new Control();
		c.AddChild(preview);

		// preview.Position = -uiCard.Size / 2;
		Vector2 scaledSize = uiCard.Size * uiCard.Scale;
		preview.Position = -scaledSize / 2;
		preview.ZIndex = 100;
		c.Modulate = new Color(1, 1, 1, 0.5f);
		SetDragPreview(c);
		
		uiCard.Hide();
		return uiCard; // Return the child uiCard, not the slot
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Left && !mouseButton.Pressed)
			{
				if (!_dragStarted)
				{
					ParentInventory.ItemSlotClicked(this);
				}
				_dragStarted = false;
			}
		}
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		DisplayServer.CursorSetShape(DisplayServer.CursorShape.CanDrop);
		return data.As<Control>() != null;
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		Control droppedCard = data.As<Control>();
		if (droppedCard == null)
			return;

		if (droppedCard is not UIBossCard && IsBossCardSlot)
        {
			droppedCard.Show();
			return;
        }

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
			if (uiCard is not UIBossCard && oldSlot.IsBossCardSlot)
            {
				droppedCard.Show();
				return;
            }
			oldSlot.RemoveChild(oldSlot.GetChild(0));
			uiCard.Owner = null;
			uiCard.Reparent(oldSlot);
			uiCard.GlobalPosition = oldSlot.GlobalPosition;
			Vector2 slotSizeOld = oldSlot.Size;
			Vector2 cardSizeOld = uiCard.Size;
			float scaleXOld = slotSizeOld.X / cardSizeOld.X;
			float scaleYOld = slotSizeOld.Y / cardSizeOld.Y;
			float scaleOld = Mathf.Min(scaleXOld, scaleYOld);

			uiCard.Scale = new Vector2(scaleOld, scaleOld);
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
		uiCard.GlobalPosition = GlobalPosition;
		Vector2 slotSize = Size;
		Vector2 cardSize = uiCard.Size;
		float scaleX = slotSize.X / cardSize.X;
		float scaleY = slotSize.Y / cardSize.Y;
		float scale = Mathf.Min(scaleX, scaleY);

		uiCard.Scale = new Vector2(scale, scale);
		uiCard.Show();
		oldSlot.EmitSignal(SignalName.CardTakenOut);
		EmitSignal(SignalName.NewCardAdded);
	}
}