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

        Global.gameManager.BattleLoop(currentKazamata.Name, "");
        BattleToUILanguageInterpreter interpreter = new(CardHolder, UICardElement.ConvertCards(Global.gameManager.GetPakli().ToArray()), UICardElement.ConvertCards(currentKazamata.KazamataCards, true));

        NextStepButton.Click += (_, _) =>
        {
            interpreter.PlayNextStep();
        };
        interpreter.finalResult += (object? _, EventArgs data) =>
        {
            bool result = ((ResultData)data).playerWon;

            SummaryPanel.IsVisible = true;
            SummaryTitle.Content = result ? "You win" : "You lose";
        };
    }
}