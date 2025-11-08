using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using DuszaCompetitionApplication.Enums;
using DuszaCompetitionApplication.GameElements;

namespace DuszaCompetitionApplication.UIResources;

class UICardElement
{
    public Card card;
    private Control cardVisual;

    public UICardElement(Card card, bool isKazamata = false)
    {
        this.card = card;

        Border cardBackground = new Border
        {
            CornerRadius = new Avalonia.CornerRadius(8),
            BorderThickness = new Avalonia.Thickness(4),
            BorderBrush = new SolidColorBrush(Color.Parse( isKazamata ? "#37124d" : "#4d3812")),
            Background = new SolidColorBrush(Color.Parse("#775e2f")),
            Padding = new Avalonia.Thickness(0),
            Margin = new Avalonia.Thickness(0),
            Width = 168,
            Height = 220
        };

        Border cardBorder = new Border
        {
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            CornerRadius = new Avalonia.CornerRadius(8),
            BorderThickness = new Avalonia.Thickness(4),
            Padding = new Avalonia.Thickness(0),
            Margin = new Avalonia.Thickness(0),
            ClipToBounds = true,
            Background = new ImageBrush(new Bitmap("./Assets/Images/Cards/cards_base.png")),
        };
        cardBackground.Child = cardBorder;



        Grid cardBodyHolder = new Grid
        {
            Margin = new Avalonia.Thickness(0),
        };
        cardBodyHolder.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        cardBodyHolder.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        cardBorder.Child = cardBodyHolder;
       
       

        Grid cardBody = new Grid
        {
            Margin = new Avalonia.Thickness(0),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        };
        cardBody.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        cardBody.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        cardBody.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        cardBody.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        cardBodyHolder.Children.Add(cardBody);



        Border cardEffectHolderBorder = new Border
        {
            CornerRadius = new Avalonia.CornerRadius(8),
            BorderThickness = new Avalonia.Thickness(4),
            Padding = new Avalonia.Thickness(0),
            Margin = new Avalonia.Thickness(0, 0, 0, 3),
            ClipToBounds = true,
            Background = card.Type == CardType.vezer ? new ImageBrush(new Bitmap(isKazamata ? "./Assets/Images/Cards/cards_enemy_leader_effect.png" : "./Assets/Images/Cards/cards_player_leader_effect.png")) : new SolidColorBrush(Colors.Transparent),
            Width = 170,
            Height = 222,
        };
        cardBodyHolder.Children.Add(cardEffectHolderBorder);

        Image entityIcon = new Image
        {
            Source = new Bitmap("./Assets/Images/Entities/Heroes/man.png"),
            Width = 80,
            Margin = new Avalonia.Thickness(0, 0, 0, 0),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
        };
        cardBodyHolder.Children.Add(entityIcon);
        

        Image elementIcon = new Image
        {
            Source = new Bitmap($"./Assets/Images/Elements/{card.Element}.png"),
            Width = 60,
            Margin = new Avalonia.Thickness(-14, -13, 0, 0)
        };
        cardBody.Children.Add(elementIcon);



        TextBlock cardName = new TextBlock
        {
            Text = card.Name,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            TextAlignment = TextAlignment.Right,
            TextWrapping = TextWrapping.Wrap,
            Width = 87,
            Margin = new Avalonia.Thickness(0,5,10,0)
        };
        Grid.SetColumn(cardName, 1);
        cardBody.Children.Add(cardName);



        Image heartIcon = new Image
        {
            Source = new Bitmap("./Assets/Images/Cards/cards_heart_base.png"),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
            Width = 37,
            Height = 47,
            Margin = new Avalonia.Thickness(5,0,0,0)
        };
        Grid.SetRow(heartIcon, 1);
        cardBody.Children.Add(heartIcon);


        Image buffedHeartEffect = new Image
        {
            Source = new Bitmap("./Assets/Images/Cards/cards_heart_buffed.png"),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
            Width = 69,
            Height = 58,
            Margin = new Avalonia.Thickness(0,0,0,0)
        };
        Grid.SetRow(buffedHeartEffect, 1);
        cardBody.Children.Add(buffedHeartEffect);



        TextBlock healthAmount = new TextBlock
        {
            Text = card.Health.ToString(),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
            TextAlignment = TextAlignment.Left,
            Margin = new Avalonia.Thickness(10, 0, 0, 10),
            FontSize = 30
        };
        Grid.SetRow(healthAmount, 1);
        cardBody.Children.Add(healthAmount);



        Image axeIcon = new Image
        {
            Source = new Bitmap("./Assets/Images/Cards/cards_axe_base.png"),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
            Width = 49,
            Height = 46,
            Margin = new Avalonia.Thickness(0,0,5,0)
        };
        Grid.SetRow(axeIcon, 1);
        Grid.SetColumn(axeIcon, 1);
        cardBody.Children.Add(axeIcon);



        Image buffedDamageEffect = new Image
        {
            Source = new Bitmap("./Assets/Images/Cards/cards_axe_buffed.png"),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
            Width = 76,
            Height = 47,
            Margin = new Avalonia.Thickness(0,0,2,17)
        };
        Grid.SetRow(buffedDamageEffect, 1);
        Grid.SetColumn(buffedDamageEffect, 1);
        cardBody.Children.Add(buffedDamageEffect);



        TextBlock attackAmount = new TextBlock
        {
            Text = card.Attack.ToString(),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
            TextAlignment = TextAlignment.Left,
            Margin = new Avalonia.Thickness(0, 0, 15, 10),
            FontSize = 30
        };
        Grid.SetRow(attackAmount, 1);
        Grid.SetColumn(attackAmount, 1);
        cardBody.Children.Add(attackAmount);


        cardVisual = cardBackground;
    }

    public Control GetCardVisual() => cardVisual;
}