using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DuszaCompetitionApplication.GameElements;
using DuszaCompetitionApplication.UIController;
using DuszaCompetitionApplication.UIResources;
using ViewModels;

namespace Views;

public partial class AllCardsMenu : UserControl
{
    public AllCardsMenu()
    {
        InitializeComponent();

        DataContext = new AllCardsMenuViewModel();
        UIController.ApplySFXToButtons([GoBackButton]);

        if (Global.gameManager?.GetAllCards() == null) return;
        
        int cardsPlacedInRow = 0;
        int totalRows = 0;

        foreach (Card card in Global.gameManager.GetAllCards())
        {
            if (CardHolder.RowDefinitions.Count <= totalRows) CardHolder.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            UICardElement currentCard = new(card);
            Control cardVisual = currentCard.GetCardVisual();
            Grid.SetColumn(cardVisual, cardsPlacedInRow);
            Grid.SetRow(cardVisual, totalRows);
            CardHolder.Children.Add(cardVisual);

            cardsPlacedInRow++;
            if (cardsPlacedInRow == 4)
            {
                cardsPlacedInRow = 0;
                totalRows++;
            }
        }
    }
}