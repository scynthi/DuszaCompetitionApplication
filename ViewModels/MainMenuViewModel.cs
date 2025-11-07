using System;
using CommunityToolkit.Mvvm.Input;
using DuszaCompetitionApplication.ViewModels;
using Views;

namespace ViewModels;

public partial class MainMenuViewModel : ViewModelBase
{
    [RelayCommand]
    private void Play()
    {
        Global.contentControl.Content = new MapScene();
    }

    [RelayCommand]
    private void Credits()
    {
        Console.WriteLine("Credits");
    }
    
    [RelayCommand]
    private void Exit()
    {
        Environment.Exit(0);
    }
}
