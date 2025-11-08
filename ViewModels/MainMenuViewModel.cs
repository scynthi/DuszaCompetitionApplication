using System;
using CommunityToolkit.Mvvm.Input;
using DuszaCompetitionApplication.Audio;
using DuszaCompetitionApplication.UIController;
using DuszaCompetitionApplication.ViewModels;
using Views;

namespace ViewModels;

public partial class MainMenuViewModel : ViewModelBase
{
    public MainMenuViewModel()
    {
        AudioManager.PlayAndLoopAudio("./Assets/Audio/Music/heart_fall_lullaby.wav");
    }

    [RelayCommand]
    private void Play()
    {
        AudioManager.FindAndPauseAudio("./Assets/Audio/Music/heart_fall_lullaby.wav");
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
