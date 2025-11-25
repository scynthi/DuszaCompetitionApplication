using Godot;
using System;
using System.Data.Common;
using System.Linq;

public partial class BossEditor : HBoxContainer
{
    [Export] public HBoxContainer NormalCardHolder;
    [Export] public HBoxContainer BossCardHolder;

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
        Utility.AddUiSimpleCardsUnderContainer(editor.gameMasterData.WorldCards, NormalCardHolder);
        Utility.AddUiBossCardsUnderContainer(editor.gameMasterData.WorldCards, BossCardHolder);
    }

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse && mouse.Pressed && mouse.ButtonIndex == MouseButton.Left && Visible)
		{
			Control hoveredItem = GetViewport().GuiGetHoveredControl();
			
			if (hoveredItem == null) return;

			if (hoveredItem.GetParent().Name.ToString() == "SimpleCard")
            {
				UICard card = (UICard)hoveredItem.GetParent().GetParent();
				if (card == null) return;

				piciMenüInstance = CreatePiciMenü();
				piciMenüInstance.Clicked += HandlePolymorphism;
				piciMenüInstance.Item = card;

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
		UICard card = (UICard)piciMenüInstance.Item;
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
        UIBossCard newBossCard = Global.gameManager.uiPackedSceneReferences.UIBossCardScene.Instantiate<UIBossCard>();
		newBossCard.EditAllCardInformation(uicard.CreateCardInstance(), evolveType, addedName);

		return newBossCard;
	}

	public PiciMenü CreatePiciMenü()
	{
		PiciMenü piciMenüInstance = Global.gameManager.uiPackedSceneReferences.PiciMenü.Instantiate<PiciMenü>();

		var uiRoot = GetTree().CurrentScene.GetNode("GameLayer/World");
		uiRoot.AddChild(piciMenüInstance);

		piciMenüInstance.editor = editor;
		piciMenüInstance.ZIndex = 999;
		piciMenüInstance.MouseFilter = MouseFilterEnum.Stop;
		piciMenüInstance.GlobalPosition = GetGlobalMousePosition();

		return piciMenüInstance;
	}
}
