using System;
using CommunityToolkit.Mvvm.Input;
using DuszaCompetitionApplication.ViewModels;
using Views;

namespace ViewModels;

public partial class DeckMenuViewModel : ViewModelBase
{

    [RelayCommand]
    public void GoBackToMap()
    {
        Global.contentControl.Content = new MapScene();
    }
}
