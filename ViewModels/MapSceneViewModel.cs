using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using DuszaCompetitionApplication.Audio;
using DuszaCompetitionApplication.ViewModels;
using Views;

namespace ViewModels;

public partial class MapSceneViewModel : ViewModelBase
{

    public MapSceneViewModel()
    {
        AudioManager.PlayAndLoopAudio("./Assets/Audio/Music/wind_loop_ambient.wav");
        RandomlyPlayAmbientMusic();
    }

    public async void RandomlyPlayAmbientMusic()
    {
        Random random = new();
        int delay = random.Next(45000, 120000);

        await Task.Run(async () => {
            while (AudioManager.FindAudioPlayer("./Assets/Audio/Music/wind_loop_ambient.wav"))
            {
                Thread.Sleep(delay);
                await AudioManager.PlayAudio("./Assets/Audio/Music/ambient_loop1.wav");
            }
        });

    }

    [RelayCommand]
    private void GoBackToMainMenu()
    {
        AudioManager.FindAndPauseAudio("./Assets/Audio/Music/wind_loop_ambient.wav");
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
