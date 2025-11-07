using System;
using System.Threading;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using DuszaCompetitionApplication.ViewModels;
using Views;

namespace ViewModels;

public partial class MapSceneViewModel : ViewModelBase
{

    [RelayCommand]
    private void GoBackToTitleScreen()
    {
        Global.contentControl.Content = new MainMenu();
    }
}
