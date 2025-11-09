using Avalonia.Controls;
using DuszaCompetitionApplication.UIResources;
using DuszaCompetitionApplication.UIController;
using System;
using ViewModels;
using DuszaCompetitionApplication.GameElements;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia;
using Avalonia.Markup.Xaml.MarkupExtensions;
using System.Linq;
using DuszaCompetitionApplication;
using DuszaCompetitionApplication.Audio;

namespace Views;

public partial class FightScene : UserControl
{
    Kazamata currentKazamata = new();
    public FightScene()
    {
        InitializeComponent();
        DataContext = new FightSceneViewModel();
        UIController.ApplySFXToButtons([GoBackButton]);
        Global.gameManager.TryReturnKazamataFromName(Global.currentKazamata, Global.gameManager.GetKazataObjects(), out currentKazamata);

        if (currentKazamata.type == DuszaCompetitionApplication.Enums.KazamataTypes.nagy)
        {
            BackgroundPanel.Bind(BackgroundProperty, Resources.GetResourceObservable("HardCaveBackground"));
        }

        foreach (UICardElement card in UICardElement.ConvertCards(Global.gameManager.GetPakli().ToArray()))
        {
            PlayerDeckHolder.Children.Add(card.GetCardVisual());
        }

        foreach (UICardElement card in UICardElement.ConvertCards(currentKazamata.KazamataCards, true))
        {
            EnemyDeckHolder.Children.Add(card.GetCardVisual());
        }

        BattleToUILanguageInterpreter interpreter = new(CardHolder, UICardElement.ConvertCards(Global.gameManager.GetPakli().ToArray()), UICardElement.ConvertCards(currentKazamata.KazamataCards, true), currentKazamata);

        NextStepButton.Click += (_, _) =>
        {
            interpreter.PlayNextStep();
        };

        interpreter.finalResult += (object? _, EventArgs data) =>
        {
            string[] result = ((ResultData)data).finalMessage;

            SummaryPanel.IsVisible = true;
            bool playerWon = result[0].Contains("nyert");

            if (playerWon)
            {
                SummaryTitle.Content = "You win";
                AudioManager.PlaySoundEffect(SoundEffectTypes.win);
            } else
            {
                SummaryTitle.Content = "You lose";
                AudioManager.PlaySoundEffect(SoundEffectTypes.lose);
            }
            
            PrizeLabel.Content = result.Length == 1 ? "" : result.Length == 2 ? $"You got: {result[1]}" : $"You got: {result[1]} for {result[2]}";
        };
    }
}