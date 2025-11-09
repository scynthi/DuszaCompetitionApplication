using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DuszaCompetitionApplication.GameElements;
using DuszaCompetitionApplication.UIController;
using DuszaCompetitionApplication.UIResources;
using ViewModels;

namespace Views;

public partial class DeckMenu : UserControl
{
    public DeckMenu()
    {
        InitializeComponent();
        DataContext = new DeckMenuViewModel();
        UIController.ApplySFXToButtons([GoBackButton]);

        Card[]? currentCollection = Global.gameManager.GetCollection().ToArray();
        Card[]? currentDeck = Global.gameManager.GetPakli().ToArray();

        foreach (UICardElement currentCardElement in UICardElement.ConvertCards(currentCollection))
        {
            if (currentDeck.Contains(currentCardElement.card)) continue;

            Control cardVisual = currentCardElement.GetCardVisual();
            PlayerInventoryHolder.Children.Add(cardVisual);
            cardVisual.PointerPressed += HandleClick;
        }


        foreach (UICardElement currentCard in UICardElement.ConvertCards(currentDeck))
        {
            PlayerDeckHolder.Children.Add(currentCard.GetCardVisual());
            currentCard.GetCardVisual().PointerPressed += HandleClick;
            
        }
    }

    private void HandleClick(object? s, EventArgs a)
    {
        Control? card = s as Control;
        if (card == null) return;

        if (PlayerDeckHolder.Children.Contains(card))
        {
            PlayerDeckHolder.Children.Remove(card);
            PlayerInventoryHolder.Children.Add(card);

            Global.gameManager.RemoveFromPakli(Global.gameManager.GetCollection().Where(x => x.Name == card.Name).First());
        }
        else if (PlayerDeckHolder.Children.Count != Math.Ceiling((decimal)Global.gameManager.GetCollection().Count / 2))
        {
            PlayerInventoryHolder.Children.Remove(card);
            PlayerDeckHolder.Children.Add(card);

            Global.gameManager.TryAddCardToPakli(Global.gameManager.GetCollection().Where(x => x.Name == card.Name).First());
        }
    }
    
}