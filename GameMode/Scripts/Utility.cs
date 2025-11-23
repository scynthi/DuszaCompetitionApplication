using Godot;
using System;
using System.Collections.Generic;

public static class Utility
{
	public static List<string> WorldObjectListToNameList<T>(List<T> objectList) where T : IWorldObject
    {
		List<string> nameList = new List<string>();
		foreach (T currObj in objectList)
			nameList.Add(currObj.Name);
		return nameList;
    }

	public static List<string> ItemListToNameList(List<IItem> itemList)
    {
		List<string> nameList = new List<string>();
		foreach (IItem currItem in itemList)
			nameList.Add(currItem.Name);
		return nameList;
    }

	public static void AddUiCardsUnderContainer(Godot.Collections.Array<Card> cards, Control container, bool clearContainerChildren = true)
    {
		if (clearContainerChildren){
			foreach (Node child in container.GetChildren())
				{
					child.QueueFree();
				}    
        }
		
		foreach(Card card in cards)
		{
			if (card is BossCard)
			{
				UIBossCard newUiCard = Global.gameManager.uiPackedSceneReferences.UIBossCardScene.Instantiate<UIBossCard>();
				newUiCard.EditAllCardInformation((BossCard)card);
				container.AddChild(newUiCard);

			} else
			{
				UICard newUiCard = Global.gameManager.uiPackedSceneReferences.UICardScene.Instantiate<UICard>();
				newUiCard.EditAllCardInformation(card);
				container.AddChild(newUiCard);
			}
		}
    }


}
