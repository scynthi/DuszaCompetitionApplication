using Godot;
using System;
using System.Data.Common;
using System.Linq;

public partial class BossEditor : HBoxContainer
{
    [Export] public HBoxContainer NormalCardHolder;
    [Export] public HBoxContainer BossCardHolder;
	[Export] PackedScene piciMenüScene;
	[Export] PackedScene UIBossCardScene;

	PiciMenü piciMenüInstance;
	Editors editor;

    public override void _Ready()
    {
        editor = (Editors)GetParent();

		editor.gameMasterData.CardDataChanged += HandleDataChange;
    }

    public override void _ExitTree()
    {
		editor.gameMasterData.CardDataChanged -= HandleDataChange;
    }

	public void HandleDataChange()
    {
		if (!Visible) return;
        Utility.AddUiCardsUnderContainer(editor.gameMasterData.WorldCards, NormalCardHolder);
        Utility.AddUiCardsUnderContainer(editor.gameMasterData.WorldCards, BossCardHolder);
    }

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse && mouse.Pressed && mouse.ButtonIndex == MouseButton.Left && Visible)
		{
			Control hoveredItem = GetViewport().GuiGetHoveredControl();
			
			if (hoveredItem == null) return;

			if (piciMenüInstance != null && hoveredItem.GetParent().Name != "PiciMenüItems")
			{
				if (IsInstanceValid(piciMenüInstance))
				{
					piciMenüInstance.Clicked -= HandlePolymorphism;
					piciMenüInstance.QueueFree();
				}
				piciMenüInstance = null;
			}

			// sry dani ide kellett raknom mert rabasztam egy gombot a cardra hogy tudjam pakolni a collection es a deck kozott check scene tree UICard es UIBossCard
			// NEZD A SOK GetParent() et

			if (hoveredItem.GetParent().Name.ToString() == "SimpleCard")
            {
				UICard card = (UICard)hoveredItem.GetParent().GetParent();
				if (card == null) return;

				piciMenüInstance = CreatePiciMenü();
				piciMenüInstance.Clicked += HandlePolymorphism;
				piciMenüInstance.card = card;

            } else if (hoveredItem.GetParent().Name.ToString() == "BossCard")
            {
                UIBossCard asBossCard = (UIBossCard)hoveredItem.GetParent().GetParent();
				if (asBossCard != null) {
					editor.gameMasterData.RemoveCardFromWorldCards(asBossCard.CreateBossCardInstance());
					return;
				}

            }
		}
	}

	public void HandlePolymorphism(PiciMenü piciMenüInstance)
    {
		UICard card = piciMenüInstance.card;
        UIBossCard bossCard;
		
        if (piciMenüInstance.option  == "hp")
        {
            bossCard = CreateBossCardInstance(card, BossDouble.HEALTH);
        } else
        {
            bossCard = CreateBossCardInstance(card, BossDouble.ATTACK);
        }

		editor.gameMasterData.AddCardToWorldCards(bossCard.CreateBossCardInstance());
    }

	private UIBossCard CreateBossCardInstance(UICard uicard, BossDouble evolveType, string addedName = "Lord ")
    {
        UIBossCard newBossCard = UIBossCardScene.Instantiate<UIBossCard>();
		newBossCard.EditAllCardInformation(uicard.CreateCardInstance(), evolveType, addedName);

		return newBossCard;
    }

	private PiciMenü CreatePiciMenü()
	{
		PiciMenü piciMenüInstance = (PiciMenü)piciMenüScene.Instantiate();

		var uiRoot = GetTree().CurrentScene.GetNode("GameLayer/World");
		uiRoot.AddChild(piciMenüInstance);

		piciMenüInstance.ZIndex = 999;
		piciMenüInstance.MouseFilter = MouseFilterEnum.Stop;
		piciMenüInstance.GlobalPosition = GetGlobalMousePosition();

		return piciMenüInstance;
	}
	
}
