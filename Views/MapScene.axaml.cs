using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ViewModels;

namespace Views;

public partial class MapScene : UserControl
{
    public MapScene()
    {
        InitializeComponent();
        DataContext = new MapSceneViewModel();

        string[] dungeons = { "sdf1", "sdsdf2" };

        for (int i = 0; i < dungeons.Length; i++)
        {
            Button dungeonButton = new();
            dungeonButton.Content = dungeons[i];
            
            DungeonPanel?.Children.Add(dungeonButton);
        }
    }
}