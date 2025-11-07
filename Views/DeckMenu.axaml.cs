using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ViewModels;

namespace Views;

public partial class DeckMenu : UserControl
{
    public DeckMenu()
    {
        InitializeComponent();
        DataContext = new DeckMenuViewModel();
    }
}