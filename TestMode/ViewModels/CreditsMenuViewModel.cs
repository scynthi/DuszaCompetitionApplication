using System;
using CommunityToolkit.Mvvm.Input;
using DuszaCompetitionApplication.ViewModels;
using Views;

namespace ViewModels;

public partial class CreditsMenuViewModel : ViewModelBase
{
    [RelayCommand]
    private void GoBackToMainMenu()
    {
        Global.contentControl.Content = new MainMenu();
    }
}
