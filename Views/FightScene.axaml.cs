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
        Card testCard = new("The guy from fortnite", 99, 99, CardElement.fold, CardType.sima);
        UICardElement UICard = new(testCard);
        CardHolder.Children.Add(UICard.GetCardVisual());
    }
}