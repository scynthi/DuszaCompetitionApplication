using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DuszaCompetitionApplication.Enums;
using DuszaCompetitionApplication.UIController;
using DuszaCompetitionApplication.UIResources;
using NAudio.Mixer;
using ViewModels;

namespace Views;

public partial class MapScene : UserControl
{
    public MapScene()
    {
        InitializeComponent();
        DataContext = new MapSceneViewModel();


        UIController.ApplySFXToButtons([GoBackButton, DeckButton, AllCardsButton]);
        
        for (int i = 0; i < Global.gameManager?.GetKazamatas().ToList().Count; i++)
        {
            Button dungeonButton = new();
            dungeonButton.Content = Global.gameManager?.GetKazamatas().ToList()[i];

            DungeonPanel.Children.Add(dungeonButton);
            dungeonButton.Click += (_, _) =>
            {
                Global.contentControl.Content = new FightScene();
            };
        }
        

        // Random random = new();

        // for (int i = 0; i < 9; i++)
        // {
        //     Global.cardsList.Add(new UICardElement(new("Test Card", i, i, CardElement.viz, random.Next(0, 2) == 1 ? CardType.vezer : CardType.sima), random.Next(0, 2) == 1));
        // }
        
        // string[] dungeons = { "Dungeon1", "Dungeon2", "Dungeon3", "Dungeon4", "Dungeon5", "Dungeon6", "Dungeon7" , "Dungeon2", "Dungeon3", "Dungeon4", "Dungeon5", "Dungeon6", "Dungeon7" , "Dungeon2", "Dungeon3", "Dungeon4", "Dungeon5", "Dungeon6", "Dungeon7" , "Dungeon2", "Dungeon3", "Dungeon4", "Dungeon5", "Dungeon6", "Dungeon7" , "Dungeon2", "Dungeon3", "Dungeon4", "Dungeon5", "Dungeon6", "Dungeon7" , "Dungeon2", "Dungeon3", "Dungeon4", "Dungeon5", "Dungeon6", "Dungeon7" , "Dungeon2", "Dungeon3", "Dungeon4", "Dungeon5", "Dungeon6", "Dungeon7" , "Dungeon2", "Dungeon3", "Dungeon4", "Dungeon5", "Dungeon6", "Dungeon7" , "Dungeon2", "Dungeon3", "Dungeon4", "Dungeon5", "Dungeon6", "Dungeon7" , "Dungeon2", "Dungeon3", "Dungeon4", "Dungeon5", "Dungeon6", "Dungeon7" };
    }
    
    
}