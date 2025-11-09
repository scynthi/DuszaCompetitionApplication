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

        foreach (Card card in Global.gameManager.GetAllCards())
        {
            UICardElement currentCard = new(card);
            Control cardVisual = currentCard.GetCardVisual();
            CardHolder.Children.Add(cardVisual);
        }
    }
}