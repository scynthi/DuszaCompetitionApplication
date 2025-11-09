using System;
using System.Collections;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
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

        if (Global.gameManager == null) return;
        UIController.ApplySFXToButtons([GoBackButton, DeckButton, AllCardsButton]);
        
        for (int i = 0; i < Global.gameManager?.GetKazamatas().ToList().Count; i++)
        {
            StackPanel portalPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center
            };
            Image portalImage = new Image
            {
                Source = new Bitmap("./Assets/Images/Portal/portal_frame.png"),
                Width = 75
            };
            portalPanel.Children.Add(portalImage);

            Button dungeonButton = new();
            dungeonButton.Content = Global.gameManager?.GetKazamatas().ToList()[i];
            portalPanel.Children.Add(dungeonButton);
            DungeonPanel.Children.Add(portalPanel);


            dungeonButton.Click += (s, _) =>
            {
                int? collection = Global.gameManager?.GetCollection()?.Count;
                string? chosenKazamata = (s as Button)?.Content?.ToString();

                if (collection == null || chosenKazamata == null) return;
                // if (Global.gameManager?.GetPakli().Count != Math.Ceiling((decimal)collection / 2)) return;

                Global.currentKazamata = chosenKazamata;
                Global.contentControl.Content = new FightScene();
            };
        }
    }
}