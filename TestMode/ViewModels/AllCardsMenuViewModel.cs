using System;
using CommunityToolkit.Mvvm.Input;
using DuszaCompetitionApplication.ViewModels;
using Views;

namespace ViewModels;

public partial class AllCardsMenuViewModel : ViewModelBase
{
    [RelayCommand]
    private void GoBackToMap()
    {
        Global.contentControl.Content = new MapScene();
    }
}
