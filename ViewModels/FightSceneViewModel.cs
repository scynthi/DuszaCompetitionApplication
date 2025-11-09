using System;
using CommunityToolkit.Mvvm.Input;
using DuszaCompetitionApplication.Audio;
using DuszaCompetitionApplication.ViewModels;
using Views;

namespace ViewModels;

public partial class FightSceneViewModel : ViewModelBase
{
    public FightSceneViewModel()
    {
        AudioManager.PlayAndLoopAudio("./Assets/Audio/Music/fight_ambiance_loop.wav");
        AudioManager.PlayAndLoopAudio("./Assets/Audio/Music/cave_loop_ambient.wav");
    }

    [RelayCommand]
    private void GoBackToMap()
    {
        AudioManager.FindAndPauseAudio("./Assets/Audio/Music/fight_ambiance_loop.wav");
        AudioManager.FindAndPauseAudio("./Assets/Audio/Music/cave_loop_ambient.wav");

        Global.contentControl.Content = new MapScene();
    }
}
