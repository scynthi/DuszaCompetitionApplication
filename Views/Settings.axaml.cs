using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Input;
using ViewModels;

namespace Views;

public partial class Settings : UserControl
{
    public Settings()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
    }
}