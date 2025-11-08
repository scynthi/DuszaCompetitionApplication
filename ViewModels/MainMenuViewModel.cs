using System;
using CommunityToolkit.Mvvm.Input;
using DuszaCompetitionApplication.ViewModels;
using Views;

namespace ViewModels;

public partial class MainMenuViewModel : ViewModelBase
{
    public MainMenuViewModel()
    {
        Global.PlayAndLoopAudio("./Assets/Audio/LakeSide Saucebook.mp3");
    }

    [RelayCommand]
    private void Play()
    {
        Global.contentControl.Content = new MapScene();
    }

    [RelayCommand]
    private void Credits()
    {
        Global.contentControl.Content = new CreditsMenu();
    }

    [RelayCommand]
    private void Settings()
    {
        Global.contentControl.Content = new Settings();
    }
    
    [RelayCommand]
    private void Exit()
    {
        Environment.Exit(0);
    }
}
