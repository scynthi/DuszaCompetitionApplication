using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using NAudio.Wave;
using Views;

namespace DuszaCompetitionApplication.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        Global.contentControl.Content = new FightScene();
        // Global.contentControl.Content = new MainMenu();
    }
}

