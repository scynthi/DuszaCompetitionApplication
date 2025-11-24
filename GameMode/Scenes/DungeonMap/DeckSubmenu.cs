using Godot;
using System;

public partial class DeckSubmenu : Control
{
    [Export] GridContainer collectionContainer;
    [Export] HBoxContainer deckContainer;


    public void ReloadCards()
    {
        Godot.Collections.Array<Card> collectionLoadList = [];
        Godot.Collections.Array<Card> deckLoadList = [];

        foreach(Card card in Global.gameManager.saverLoader.currSaveFile.player.Collection)
        {
            if (Global.gameManager.saverLoader.currSaveFile.player.Deck.Contains(card)){
                
                deckLoadList.Add(card);
            }
            else
            {
                collectionLoadList.Add(card);
            }
        }

        Utility.AddUiCardsUnderContainer(collectionLoadList , collectionContainer);        
        Utility.AddUiCardsUnderContainer(deckLoadList , deckContainer);

        foreach (Control child in collectionContainer.GetChildren())
        {
            if (child is UICard)
            {
                
            } else if (child is UIBossCard)
            {
                
            }
        }



    }
}
