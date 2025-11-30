using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class Inventory : PanelContainer
{
	[Signal]
	public delegate void DeckIsFullEventHandler();
	[Signal]
	public delegate void DeckIsNotFullEventHandler();

	[Export] PackedScene ItemSlotScene;
	[Export] GridContainer MainContainer;
	[Export] Inventory ConnectedInventory;
	[Export] public int AmountOfCols = 6;
	[Export] public float ScaleDown = 1; 
	public bool IsDungeonDeck = false;
	public bool IsPlayerDeck = false;

	List<ItemSlot> ItemSlots = new List<ItemSlot>();

	public override void _Ready()
	{
		base._Ready();
	}

	public void RemakePanelItems(int amount = 0, bool IsSmall = false, List<Card> Collection = null, bool IsBossCardNeeded = false)
    {
		if (IsSmall)
			MainContainer.Columns = 4;
		else
			MainContainer.Columns = AmountOfCols;

		for (int i = MainContainer.GetChildCount() - 1; i >= 0; i--)
		{
			Node child = MainContainer.GetChild(i);
			MainContainer.RemoveChild(child);
			child.QueueFree();
		
		}
		ItemSlots.Clear();

        UICard tempUICard = Global.gameManager.uiPackedSceneReferences.UICardScene.Instantiate<UICard>();

		if (Collection != null)
        {
            amount = Collection.Count;
        }

		Godot.Vector2 size = tempUICard.CustomMinimumSize;
		List<string> otherInventoryCards = Utility.WorldObjectListToNameList(ConnectedInventory.ReturnContents());
		for (int i = 0; i < amount; i++)
		{
			ItemSlot itemSlot = ItemSlotScene.Instantiate<ItemSlot>();
			itemSlot.CustomMinimumSize = size / ScaleDown;
			itemSlot.ParentInventory = this;

			if (i == amount - 1 && IsBossCardNeeded)
            {
                itemSlot.IsBossCardSlot = true;
            }
			
			if (Collection != null)
            {
                Card card = Collection[i];

				if (IsPlayerDeck && !otherInventoryCards.Contains(card.Name))
                {
                    if (card is BossCard)
                {
					UIBossCard bossUICard = Global.gameManager.uiPackedSceneReferences.UIBossCardScene.Instantiate<UIBossCard>();
					bossUICard.SetOwnerCard(card as BossCard);
					itemSlot.AddChild(bossUICard);
					// Vector2 slotSize = itemSlot.Size;
					// Vector2 cardSize = bossUICard.Size;
					// float scaleX = slotSize.X / cardSize.X;
					// float scaleY = slotSize.Y / cardSize.Y;
					// float scale = Mathf.Min(scaleX, scaleY);

					// bossUICard.Scale = new Vector2(scale, scale);
					itemSlot.uiCard = bossUICard;
					bossUICard.MouseFilter = Control.MouseFilterEnum.Ignore;
					bossUICard.EditAllCardInformation(bossUICard.OwnerCard);
                }
                else
                {
					UICard uiCard = Global.gameManager.uiPackedSceneReferences.UICardScene.Instantiate<UICard>();
					uiCard.SetOwnerCard(card);
					itemSlot.AddChild(uiCard);
					// Vector2 slotSize = itemSlot.Size;
					// Vector2 cardSize = uiCard.Size;
					// float scaleX = slotSize.X / cardSize.X;
					// float scaleY = slotSize.Y / cardSize.Y;
					// float scale = Mathf.Min(scaleX, scaleY);

					// uiCard.Scale = new Vector2(scale, scale);
					itemSlot.uiCard = uiCard;
					uiCard.MouseFilter = Control.MouseFilterEnum.Ignore;
					uiCard.EditAllCardInformation(uiCard.OwnerCard);
                }
                }
            }

			MainContainer.AddChild(itemSlot);
			ItemSlots.Add(itemSlot);
			itemSlot.NewCardAdded += NewCardAdded;
			itemSlot.CardTakenOut += NewCardAdded;
		}
		ShiftLeft();
    }

	public void ClearCards()
    {
        for (int i = MainContainer.GetChildCount() - 1; i >= 0; i--)
		{
			ItemSlot Container = MainContainer.GetChild(i) as ItemSlot;
			if (Container.GetChildCount() > 0)
            {
                Node Card = Container.GetChild(0);
				Container.RemoveChild(Card);
				Container.uiCard = null;
				Card.QueueFree();
            }
		}
    }

	public void ItemSlotClicked(ItemSlot itemSlot)
    {
        if (itemSlot.uiCard == null)
			return;

		int firstEmpty = ConnectedInventory.FindFirstEmptySpace();

		if (firstEmpty == -1)
			return;

		ItemSlot otherSlot = ConnectedInventory.ItemSlots[firstEmpty];

		if (otherSlot.IsBossCardSlot && itemSlot.uiCard is not UIBossCard)
			return;

		itemSlot.uiCard.Reparent(otherSlot);
		otherSlot.uiCard = itemSlot.uiCard;
		itemSlot.uiCard = null;
		otherSlot.uiCard.GlobalPosition = otherSlot.GlobalPosition;

		Vector2 slotSize = otherSlot.Size;
		Vector2 cardSize = otherSlot.uiCard.Size;
		float scaleX = slotSize.X / cardSize.X;
		float scaleY = slotSize.Y / cardSize.Y;
		float scale = Mathf.Min(scaleX, scaleY);
		otherSlot.uiCard.Scale = new Vector2(scale, scale);

		NewCardAdded();
		ConnectedInventory.NewCardAdded();
    }

	private void NewCardAdded()
    {
        ShiftLeft();
		// Global.gameManager.saverLoader.currSaveFile.player.SetDeck(ReturnContents());
		if (ReturnContents().Count == ItemSlots.Count)
        {
            EmitSignal(SignalName.DeckIsFull);
        }
        else
        {
            EmitSignal(SignalName.DeckIsNotFull);
        }
		if (IsPlayerDeck)
        {
            Global.gameManager.saverLoader.currSaveFile.player.SetDeck(ReturnContents());
        }
    }

	private void ShiftLeft()
    {
        for (int i = 0; i < ItemSlots.Count - (IsDungeonDeck ? 1 : 0); i++)
        {
            if (ItemSlots[i].uiCard == null)
            {
				int nextNonNullCard = FindNextNullCard(i);
				if (nextNonNullCard == -1) return;
                ItemSlot foundItemSlot = ItemSlots[nextNonNullCard];
				ItemSlot currentItemSlot = ItemSlots[i];

				var uiCard = foundItemSlot.uiCard;
				foundItemSlot.RemoveChild(foundItemSlot.GetChild(0));
				foundItemSlot.uiCard = null;

				currentItemSlot.AddChild(uiCard);
				currentItemSlot.uiCard = uiCard;
            }
        }
    }

	public int FindFirstEmptySpace()
    {
		for (int i = 0; i < ItemSlots.Count; i++)
        {
            if (ItemSlots[i].uiCard == null)
            {
                return i;
            }
        }
        return -1;
    }

	private int FindNextNullCard(int startIndex)
    {
        for (int i = startIndex; i < ItemSlots.Count - (IsDungeonDeck ? 1 : 0); i++)
        {
            if (ItemSlots[i].uiCard != null)
            {
                return i;
            }
        }
		return -1;
    }

	public List<Card> ReturnContents()
    {
		List<Card> cards = new List<Card>();
		foreach (ItemSlot itemSlot in ItemSlots)
        {
            if (itemSlot.uiCard == null)
				break;
			if (itemSlot.uiCard is UICard)
            {
                UICard uICard = itemSlot.uiCard as UICard;
				cards.Add(uICard.OwnerCard);
            }				
			else if (itemSlot.uiCard is UIBossCard)
            {
				UIBossCard uICard = itemSlot.uiCard as UIBossCard;
				cards.Add(uICard.OwnerCard);
            }
        }
		return cards;
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
