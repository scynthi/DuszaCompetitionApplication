using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DuszaCompetitionApplication.UIController;
using ViewModels;

namespace Views;

public partial class CreditsMenu : UserControl
{
    public CreditsMenu()
    {
        InitializeComponent();
        DataContext = new CreditsMenuViewModel();
        UIController.ApplySFXToButtons([GoBackButton]);

    }
}