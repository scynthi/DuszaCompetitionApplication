using System;
using System.Threading;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using DuszaCompetitionApplication.ViewModels;
using Views;

namespace ViewModels;

public partial class MapSceneViewModel : ViewModelBase
{

    public MapSceneViewModel()
    {
        Global.PlayAndLoopAudio("./Assets/Audio/Explorativ High jinks.mp3");
    }

    [RelayCommand]
    private void GoBackToMainMenu()
    {
        Global.contentControl.Content = new MainMenu();
    }

    [RelayCommand]
    private void GoToDeckMenu()
    {
        Global.contentControl.Content = new DeckMenu();
    }

    [RelayCommand]
    private void GoToAllCardsMenu()
    {
        Global.contentControl.Content = new AllCardsMenu();
    }
}
