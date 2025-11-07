using Avalonia.Controls;

namespace DuszaCompetitionApplication.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Global.mainWindow = this;
        Global.contentControl = ContentControlItem;
    }
}