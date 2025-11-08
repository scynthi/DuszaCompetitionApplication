using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using DuszaCompetitionApplication.ViewModels;
using Views;

namespace ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [RelayCommand]
    private void GoBackToMainMenu()
    {
        Global.contentControl.Content = new MainMenu();
    }

    [RelayCommand]
    private void ApplySettings()
    {
        Global.MusicVolume = Global.getElementByName<Slider>("VolumeSlider")?.Value == null ? 0 : (float?)Global.getElementByName<Slider>("VolumeSlider")?.Value;
    }
}
