using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DuszaCompetitionApplication.UIController;
using ViewModels;

namespace Views;

public partial class MapScene : UserControl
{
    public MapScene()
    {
        InitializeComponent();
        DataContext = new MapSceneViewModel();
        UIController.ApplySFXToButtons([GoBackButton, DeckButton, AllCardsButton]);

        string[] dungeons = { "Dungeon1", "Dungeon2", "Dungeon3", "Dungeon4", "Dungeon5", "Dungeon6", "Dungeon7" };

        for (int i = 0; i < dungeons.Length; i++)
        {
            Button dungeonButton = new();
            dungeonButton.Content = dungeons[i];

            DungeonPanel?.Children.Add(dungeonButton);
        }
    }
    
    
}