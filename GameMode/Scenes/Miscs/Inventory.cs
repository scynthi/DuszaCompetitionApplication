using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;

public partial class Inventory : PanelContainer
{

	[Export] PackedScene ItemSlotScene;
	[Export] GridContainer MainContainer;
	[Export] int AmountOfCols = 6;
	public bool IsBossCardNeeded = false;
	[Export] public float ScaleDown = 1; 

	List<ItemSlot> ItemSlots = new List<ItemSlot>();

	public override void _Ready()
	{
		base._Ready();
	}

	public void RemakePanelItems(int amount, bool IsSmall = false, List<Card> Collection = null)
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

        UICard uiCard =  Global.gameManager.uiPackedSceneReferences.UICardScene.Instantiate<UICard>();
		uiCard.SetOwnerCard(new Card("Aple", 5, 5, CardElements.EARTH));

		if (Collection != null)
        {
            amount = Collection.Count;
        }

		Godot.Vector2 size = uiCard.Size;
		for (int i = 0; i < amount; i++)
		{
			ItemSlot itemSlot = ItemSlotScene.Instantiate<ItemSlot>();
			itemSlot.CustomMinimumSize = size / ScaleDown;

			if (i == amount - 1 && IsBossCardNeeded)
            {
                itemSlot.IsBossCardSlot = true;
            }
			
			// if (i == 3 || i == 2)
            // {
			// 	UIBossCard buiCard = Global.gameManager.uiPackedSceneReferences.UIBossCardScene.Instantiate<UIBossCard>();
			// 	buiCard.SetOwnerCard(new BossCard("Aple" + i, 5, 5, CardElements.EARTH, "uid://m4uonlvhx0mu"));
			// 	itemSlot.AddChild(buiCard);
			// 	itemSlot.uiCard = buiCard;
			// 	buiCard.MouseFilter = Control.MouseFilterEnum.Ignore;
			// 	buiCard.EditAllCardInformation(buiCard.OwnerCard);
            // }
			// if (i == 1)
            // {
            //     UICard buiCard = Global.gameManager.uiPackedSceneReferences.UICardScene.Instantiate<UICard>();
			// 	buiCard.SetOwnerCard(new Card("Aple" + i, 5, 5, CardElements.EARTH, "uid://m4uonlvhx0mu"));
			// 	itemSlot.AddChild(buiCard);
			// 	itemSlot.uiCard = buiCard;
			// 	buiCard.MouseFilter = Control.MouseFilterEnum.Ignore;
			// 	buiCard.EditAllCardInformation(buiCard.OwnerCard);
            // }

			MainContainer.AddChild(itemSlot);
			ItemSlots.Add(itemSlot);
			itemSlot.NewCardAdded += NewCardAdded;
			itemSlot.CardTakenOut += NewCardAdded;
		}
		ShiftLeft();

		GD.Print($"MainContainer child count: {MainContainer.GetChildCount()}");
		GD.Print($"MainContainer visible: {MainContainer.Visible}");
		GD.Print($"MainContainer size: {MainContainer.Size}");
    }

	private void NewCardAdded()
    {
        ShiftLeft();
		// Global.gameManager.saverLoader.currSaveFile.player.SetDeck(ReturnContents());
    }

	private void ShiftLeft()
    {
        for (int i = 0; i < ItemSlots.Count - 1; i++)
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

	private int FindNextNullCard(int startIndex)
    {
        for (int i = startIndex; i < ItemSlots.Count; i++)
        {
            if (ItemSlots[i].uiCard != null)
            {
                return i;
            }
        }
		return -1;
    }

	public void ReturnContents()
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
			// return new List<Card>();
			
        }
		foreach (Card card in cards)
			if (card is BossCard)
				GD.Print(card.Name + "BossCard");
			else
				GD.Print(card.Name + "NOTBOSS");
		// return cards;
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
