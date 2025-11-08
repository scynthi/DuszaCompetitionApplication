using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DuszaCompetitionApplication.Enums;
using DuszaCompetitionApplication.UIResources;
using DuszaCompetitionApplication.GameElements;

namespace Views;

public partial class FightScene : UserControl
{
    public FightScene()
    {
        InitializeComponent();
        Card testCard = new("The Hunter", 10, 10, CardElement.fold, CardType.sima);
        UICardElement UICard = new(testCard);
        CardHolder.Children.Add(UICard.GetCardVisual());
    }
}