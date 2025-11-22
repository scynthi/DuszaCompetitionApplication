using Godot;
using System;

public partial class BossEditor : HBoxContainer
{
    [Export] HBoxContainer NormalCardHolder;
    [Export] HBoxContainer BossCardHolder;
	[Export] PackedScene piciMenüScene;

	PiciMenü piciMenüInstance;


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse && mouse.Pressed && mouse.ButtonIndex == MouseButton.Left)
		{
			Control hoveredItem = GetViewport().GuiGetHoveredControl();

			if (hoveredItem == null) return;
			if (hoveredItem.Name.ToString() != "ClickCatcher") return;

			UICard card = (UICard)hoveredItem.GetParent();

			if (card == null) return;

			if (card.isBoss)
            {
                card.QueueFree();
				return;
            }

			if (piciMenüInstance != null)
			{
				if (IsInstanceValid(piciMenüInstance))
				{
					piciMenüInstance.Clicked -= HandlePolymorphism;
					piciMenüInstance.QueueFree();
				}

				piciMenüInstance = null;
			}

			piciMenüInstance = CreatePiciMenü();
			piciMenüInstance.Clicked += HandlePolymorphism;
			piciMenüInstance.card = card;
		}
	}

    public void AddCardToList(UICard card)
    {
        NormalCardHolder.AddChild(SpawnCardInstance(card));
    }

	public void HandlePolymorphism(PiciMenü piciMenüInstance)
    {
		UICard card = piciMenüInstance.card;
		UICard bossCard = SpawnCardInstance(card, true);
		BossCardHolder.AddChild(bossCard);

		if (piciMenüInstance.option == "hp")
		{
			bossCard.EditHealth(bossCard.CardHealth*2);
		} else
		{
			bossCard.EditDamage(bossCard.CardDamage*2);
		}
    }

	private UICard SpawnCardInstance(UICard card, bool isBoss = false)
    {
		UICard newCard = ((PackedScene)GD.Load("uid://dk32ss75ce3lw")).Instantiate<UICard>();
		newCard.EditAllCardInformation(card);
		newCard.isBoss = isBoss;
		newCard.EditEffect();

		return newCard;
    }

	private PiciMenü CreatePiciMenü()
	{
		PiciMenü piciMenüInstance = (PiciMenü)piciMenüScene.Instantiate();

		// find your UI root (adjust path to match your scene)
		var uiRoot = GetTree().CurrentScene.GetNode("GameLayer/World");

		uiRoot.AddChild(piciMenüInstance);

		piciMenüInstance.GlobalPosition = GetGlobalMousePosition();
		piciMenüInstance.ZIndex = 999;   // always above other UI
		piciMenüInstance.MouseFilter = MouseFilterEnum.Stop; // capture clicks

		return piciMenüInstance;
	}
}
