using Godot;

public static class PiciMenüHandler
{
    private static PiciMenü piciMenüInstance;

	public static void HandlePiciMenüCreation(InputEvent @event)
	{
        if(Global.masterEditor.CurrentMenu is CardEditor) return;
        if(Global.masterEditor.CurrentMenu is DungeonEditor) return;
        if(Global.masterEditor.CurrentMenu is SaveMenu) return;

		if (@event is InputEventMouseButton mouse && mouse.Pressed && mouse.ButtonIndex == MouseButton.Left)
		{
			Control hoveredItem = Global.masterEditor.CurrentMenu.GetViewport().GuiGetHoveredControl();
			
			if (hoveredItem == null) return;
            string hoveredItemName = hoveredItem.GetParent().Name.ToString();

            switch(hoveredItemName)
            {
                case "SimpleCard":
                    UICard card = (UICard)hoveredItem.GetParent().GetParent();
                    if (card == null) break;

                    piciMenüInstance = CreatePiciMenü();
                    piciMenüInstance.Item = card;
                    piciMenüInstance.Clicked += HandlePolymorphism;
                    break;
                case "BossCard":
                    UIBossCard asBossCard = (UIBossCard)hoveredItem.GetParent().GetParent();
                    if (asBossCard == null) break;

                    piciMenüInstance = CreatePiciMenü();
                    piciMenüInstance.Item = asBossCard;
                    break;
                case "UIDungeon":
                    UIDungeon dungeon = (UIDungeon)hoveredItem.GetParent();
                    if (dungeon == null) break;

                    piciMenüInstance = CreatePiciMenü();
                    piciMenüInstance.Item = dungeon;
                    break;
            }   
		}
	}

	private static void HandlePolymorphism(PiciMenü piciMenüInstance)
    {
		UICard card = (UICard)piciMenüInstance.Item;
        UIBossCard bossCard;

		
        if (piciMenüInstance.option  == "hp")
        {
            bossCard = CreateBossCardInstance(card, BossDouble.HEALTH, piciMenüInstance.PrefixName.Text);
        } else
        {
            bossCard = CreateBossCardInstance(card, BossDouble.ATTACK, piciMenüInstance.PrefixName.Text);
        }
        if (!Global.masterEditor.gameMasterData.TestCard(bossCard.CreateBossCardInstance())) return;

		Global.masterEditor.gameMasterData.AddCardToWorldCards(bossCard.CreateBossCardInstance());
    }

	private static UIBossCard CreateBossCardInstance(UICard uicard, BossDouble evolveType, string addedName)
    {
        UIBossCard newBossCard = Global.gameManager.uiPackedSceneReferences.UIBossCardScene.Instantiate<UIBossCard>();
		newBossCard.EditAllCardInformation(uicard.CreateCardInstance(), evolveType, addedName);

		return newBossCard;
	}

	public static PiciMenü CreatePiciMenü()
	{
		PiciMenü piciMenüInstance = Global.gameManager.uiPackedSceneReferences.PiciMenü.Instantiate<PiciMenü>();

		var uiRoot = Global.masterEditor.CurrentMenu.GetTree().CurrentScene.GetNode("GameLayer/World");
		uiRoot.AddChild(piciMenüInstance);

		piciMenüInstance.ZIndex = 999;
		piciMenüInstance.MouseFilter = Control.MouseFilterEnum.Stop;
		piciMenüInstance.GlobalPosition = Global.masterEditor.CurrentMenu.GetGlobalMousePosition();

		return piciMenüInstance;
	}
}
