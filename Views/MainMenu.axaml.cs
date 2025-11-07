using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ViewModels;

namespace Views;

public partial class MainMenu : UserControl
{
    public MainMenu()
    {
        InitializeComponent();
        DataContext = new MainMenuViewModel();
    }
}