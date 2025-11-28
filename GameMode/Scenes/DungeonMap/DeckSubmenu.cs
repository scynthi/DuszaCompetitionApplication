using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DeckSubmenu : Control
{
    [Export] GridContainer collectionContainer;
    [Export] HBoxContainer deckContainer;

    [Export] Label moneyLabel;
    [Export] Label xpLabel;



    public void ReloadCards()
    {
        ReloadXPAndMoneyLabel();
        List<Card> collectionLoadList = [];
        List<Card> deckLoadList = [];

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

        var allCards = deckContainer.GetChildren()
            .Concat(collectionContainer.GetChildren())
            .ToList();

        foreach (Control child in allCards)
        {
            if (child is UICard card)
            {
                card.CardClicked += _OnCardPressed;

            } else if (child is UIBossCard bossCard)
            {
                bossCard.CardClicked += _OnCardPressed;
            }
        }
    }


    public void _OnCardPressed(Control card)
    {
        Control cardParent = card.GetParent<Control>();
        
        Player player = Global.gameManager.saverLoader.currSaveFile.player;

        string cardName = card switch
        {
            UICard uiCard => uiCard.CardName,
            UIBossCard uiBossCard => uiBossCard.CardName,
            _ => null
        };

        if (cardName == null) return;

        bool movingToDeck = cardParent != deckContainer;

        if (movingToDeck)
        {
            int maxDeckSize = (int)Math.Ceiling(player.Collection.Count / 2.0);
            if (deckContainer.GetChildCount() >= maxDeckSize) return;
        }

        Control targetContainer = movingToDeck ? deckContainer : collectionContainer;
        card.Reparent(targetContainer);

        if (movingToDeck)
        {
            player.TryAddToDeck(cardName);
        }
        else
        {
            player.TryRemoveFromDeck(cardName);
        }
    }

    public void ReloadXPAndMoneyLabel()
    {
        moneyLabel.Text = Convert.ToString(Global.gameManager.saverLoader.currSaveFile.player.Money);
        xpLabel.Text = Convert.ToString(Global.gameManager.saverLoader.currSaveFile.player.Xp);
        GD.Print("asasdasdasdasd");
    }
}
