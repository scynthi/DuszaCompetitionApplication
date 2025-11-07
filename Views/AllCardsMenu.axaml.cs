using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ViewModels;

namespace Views;

public partial class AllCardsMenu : UserControl
{
    public AllCardsMenu()
    {
        InitializeComponent();
        DataContext = new AllCardsMenuViewModel();
    }
}