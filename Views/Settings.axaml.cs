using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Input;
using DuszaCompetitionApplication.Audio;
using DuszaCompetitionApplication.UIController;
using ViewModels;

namespace Views;

public partial class Settings : UserControl
{
    public Settings()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
        UIController.ApplySFXToButtons([ApplyButton, GoBackButton]);
    }
}