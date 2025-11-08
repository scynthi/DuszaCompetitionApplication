using Avalonia.Controls;
using DuszaCompetitionApplication.UIResources;
using DuszaCompetitionApplication.UIController;
using System;
using ViewModels;

namespace Views;

public partial class FightScene : UserControl
{
    public FightScene()
    {
        InitializeComponent();
        DataContext = new FightSceneViewModel();

        foreach (UICardElement card in Global.cardsList)
        {
            CardHolder.Children.Add(card.GetCardVisual());
        }
        Global.cardsList.Clear();

        UIController.ApplySFXToButtons([GoBackButton]);
    }
}