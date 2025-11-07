using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ViewModels;

namespace Views;

public partial class CreditsMenu : UserControl
{
    public CreditsMenu()
    {
        InitializeComponent();
        DataContext = new CreditsMenuViewModel();
    }
}