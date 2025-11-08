using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DuszaCompetitionApplication.UIController;
using ViewModels;

namespace Views;

public partial class DeckMenu : UserControl
{
    public DeckMenu()
    {
        InitializeComponent();
        DataContext = new DeckMenuViewModel();
        UIController.ApplySFXToButtons([GoBackButton]);
    }
}