using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DuszaCompetitionApplication.Audio;
using DuszaCompetitionApplication.UIController;
using ViewModels;

namespace Views;

public partial class MainMenu : UserControl
{
    public MainMenu()
    {
        InitializeComponent();
        DataContext = new MainMenuViewModel();
        
        UIController.ApplySFXToButtons([ PlayButton, SettingsButton, CreditsButton, ExitButton ]);
    }
}