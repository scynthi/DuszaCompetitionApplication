using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DuszaCompetitionApplication.UIController;
using ViewModels;

namespace Views;

public partial class AllCardsMenu : UserControl
{
    public AllCardsMenu()
    {
        InitializeComponent();
        DataContext = new AllCardsMenuViewModel();
        UIController.ApplySFXToButtons([GoBackButton]);
    }
}