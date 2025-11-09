using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using DuszaCompetitionApplication.Audio;
using DuszaCompetitionApplication.Enums;
using DuszaCompetitionApplication.GameElements;

namespace DuszaCompetitionApplication.UIResources;

public class UICardElement
{
    public static UICardElement[] ConvertCards(Card[]? cards, bool isKazamata = false)
    {
        if (cards == null) return [];

        var convertedCards = new UICardElement[cards.Length];

        for (int i = 0; i < cards.Length; i++)
        {
            convertedCards[i] = new UICardElement(cards[i], isKazamata);
        }

        return convertedCards;
    }
    public Card card;
    private Control cardVisual;

    private Random random = new();

    public UICardElement(Card card, bool isKazamata = false)
    {
        card.isKazamata = isKazamata;
        this.card = card;

        Border cardBase = new Border
        {
            Name = card.Name,
            CornerRadius = new Avalonia.CornerRadius(8),
            BorderThickness = new Avalonia.Thickness(4),
            BorderBrush = new SolidColorBrush(Color.Parse(isKazamata ? "#37124d" : "#4d3812")),
            Background = new SolidColorBrush(Color.Parse(isKazamata ? "#49255e" : "#775e2f")),
            Padding = new Avalonia.Thickness(0),
            Margin = new Avalonia.Thickness(0),
            Width = 168,
            Height = 220
        };

        Grid cardEntityAndBaseholder = new Grid
        {
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            Margin = new Avalonia.Thickness(0),
        };
        cardEntityAndBaseholder.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        cardEntityAndBaseholder.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        cardBase.Child = cardEntityAndBaseholder;
        cardEntityAndBaseholder.Classes.Add("UICards");



        Image entityIcon = new Image
        {

            //Source = new Bitmap(isKazamata ? random.Next(0,2) == 0 ? "./Assets/Images/Entities/Enemies/char_wendigo.png" : "./Assets/Images/Entities/Enemies/char_gobellin.png" : random.Next(0,2) == 0 ? "./Assets/Images/Entities/Heroes/char_hunter.png" : "./Assets/Images/Entities/Heroes/char_huntress.png"),
            Source = new Bitmap(card.cardIconPath != "" ? card.cardIconPath 
                : isKazamata ? random.Next(0, 2) == 0 ? "./Assets/Images/Entities/Enemies/char_wendigo.png" : "./Assets/Images/Entities/Enemies/char_gobellin.png" : random.Next(0, 2) == 0 ? "./Assets/Images/Entities/Heroes/char_hunter.png" : "./Assets/Images/Entities/Heroes/char_huntress.png"),

            Width = 150,
            Margin = new Avalonia.Thickness(0, 0, 0, 5),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom
        };
        cardEntityAndBaseholder.Children.Add(entityIcon);


        Border cardBorder = new Border
        {
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            CornerRadius = new Avalonia.CornerRadius(8),
            BorderThickness = new Avalonia.Thickness(4),
            Padding = new Avalonia.Thickness(0),
            Margin = new Avalonia.Thickness(0, 0, 0, 0),
            ClipToBounds = true,
            Background = new ImageBrush(new Bitmap("./Assets/Images/Cards/cards_base.png")),
        };
        cardEntityAndBaseholder.Children.Add(cardBorder);



        Grid cardBodyHolder = new Grid
        {
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            Margin = new Avalonia.Thickness(0)
        };
        cardBodyHolder.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        cardBodyHolder.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        cardBorder.Child = cardBodyHolder;



        Grid cardBody = new Grid
        {
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            Margin = new Avalonia.Thickness(0)
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
            Margin = new Avalonia.Thickness(0, 0, 0, 0),
            ClipToBounds = true,
            Background = card.Type == CardType.vezer ? new ImageBrush(new Bitmap(isKazamata ? "./Assets/Images/Cards/cards_enemy_leader_effect.png" : "./Assets/Images/Cards/cards_player_leader_effect.png")) : new SolidColorBrush(Colors.Transparent),
            Width = 176,
            Height = 228,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
        };
        cardEntityAndBaseholder.Children.Add(cardEffectHolderBorder);


        Image elementIcon = new Image
        {
            Source = new Bitmap($"./Assets/Images/Elements/{card.Element}.png"),
            Width = 50,
            Margin = new Avalonia.Thickness(0, 0, 0, 0),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top
        };
        cardBody.Children.Add(elementIcon);



        TextBlock cardName = new TextBlock
        {
            Text = card.Name,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
            TextAlignment = TextAlignment.Right,
            TextWrapping = TextWrapping.Wrap,
            Width = 87,
            Margin = new Avalonia.Thickness(0, 5, 10, 0)
        };
        Grid.SetColumn(cardName, 1);
        cardBody.Children.Add(cardName);



        Image heartIcon = new Image
        {
            Source = new Bitmap("./Assets/Images/Cards/cards_heart_base.png"),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
            Width = 38,
            Height = 48,
            Margin = new Avalonia.Thickness(5, 0, 0, 0)
        };
        Grid.SetRow(heartIcon, 1);
        cardBody.Children.Add(heartIcon);


        if (card.HealthChanged)
        {
            Image buffedHeartEffect = new Image
            {
                Source = new Bitmap("./Assets/Images/Cards/cards_heart_buffed.png"),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
                Width = 69,
                Height = 58,
                Margin = new Avalonia.Thickness(0, 0, 0, 5)

            };
            Grid.SetRow(buffedHeartEffect, 1);
            cardBody.Children.Add(buffedHeartEffect);
        }


        TextBlock healthAmount = new TextBlock
        {
            Text = card.Health.ToString(),
            Name = "HealthAmount",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
            TextAlignment = TextAlignment.Center,
            Margin = new Avalonia.Thickness(0, 0, 11 / card.Health.ToString().Length, 10),
            FontSize = 30
        };
        Grid.SetRow(healthAmount, 1);
        cardBody.Children.Add(healthAmount);



        Image axeIcon = new Image
        {
            Source = new Bitmap("./Assets/Images/Cards/cards_axe_base.png"),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
            Width = 50,
            Height = 46,
            Margin = new Avalonia.Thickness(0, 0, 5, 0)
        };
        Grid.SetRow(axeIcon, 1);
        Grid.SetColumn(axeIcon, 1);
        cardBody.Children.Add(axeIcon);


        if (card.AttackChanged)
        {
            Image buffedDamageEffect = new Image
            {
                Source = new Bitmap("./Assets/Images/Cards/cards_axe_buffed.png"),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
                Width = 76,
                Height = 48,
                Margin = new Avalonia.Thickness(0, 0, -5, 25)
            };
            Grid.SetRow(buffedDamageEffect, 1);
            Grid.SetColumn(buffedDamageEffect, 1);
            cardBody.Children.Add(buffedDamageEffect);
        }


        TextBlock attackAmount = new TextBlock
        {
            Text = card.Attack.ToString(),
            Name = "AttackAmount",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
            TextAlignment = TextAlignment.Right,
            Margin = new Avalonia.Thickness(0, 0, 25 / card.Attack.ToString().Length, 10),
            FontSize = 30
        };

        Grid.SetRow(attackAmount, 1);
        Grid.SetColumn(attackAmount, 1);
        cardBody.Children.Add(attackAmount);


        cardVisual = cardBase;


    }

    public void EditHealth(string value)
    {
        cardVisual.GetVisualDescendants().Where(x => x.Name == "HealthAmount").OfType<TextBlock>().First().Text = value;
    }

    public void EditAttack(string value)
    {
        cardVisual.GetVisualDescendants().Where(x => x.Name == "AttackAmount").OfType<TextBlock>().First().Text = value;
    }

    public Control GetCardVisual() => cardVisual;

}

public class ResultData : EventArgs
{
    public string[] finalMessage;

    public ResultData(string[] message)
    {
        this.finalMessage = message;
    }
}

public class BattleToUILanguageInterpreter
{
    public event EventHandler? finalResult;
    private List<List<string>> battleLog = new();
    private List<string> rawBattleLog = new();
    private Control parentControl;
    private UICardElement[] playerDeck;
    private UICardElement[] enemyDeck;

    StackPanel enemyDeckControl;
    StackPanel enemyCardControl;
    StackPanel playerDeckControl;
    StackPanel playerCardControl;

    Kazamata currentKazamata;
    public BattleToUILanguageInterpreter(Control parentControl, UICardElement[] playerDeck, UICardElement[] enemyDeck, Kazamata currentKazamata)
    {
        Global.gameManager.BattleLoopUI(currentKazamata.Name, "", out rawBattleLog);

        this.parentControl = parentControl;
        this.playerDeck = playerDeck;
        this.enemyDeck = enemyDeck;
        this.currentKazamata = currentKazamata;

        enemyDeckControl = parentControl.GetVisualDescendants().OfType<StackPanel>().Where(x => x.Name == "EnemyDeckHolder").First();
        enemyCardControl = parentControl.GetVisualDescendants().OfType<StackPanel>().Where(x => x.Name == "EnemyCardHolder").First();
        playerDeckControl = parentControl.GetVisualDescendants().OfType<StackPanel>().Where(x => x.Name == "PlayerDeckHolder").First();
        playerCardControl = parentControl.GetVisualDescendants().OfType<StackPanel>().Where(x => x.Name == "PlayerCardHolder").First();

        for (int i = 0; i < rawBattleLog.Count; i++)
        {
            if (rawBattleLog[i] == "" || rawBattleLog[i].StartsWith("harc") || rawBattleLog[i] == "\n")
            {
                continue;
            }
            battleLog.Add(rawBattleLog[i].Split(";").ToList());
        }
        Console.WriteLine("---------------------------");
        Utility.PrintArray(battleLog);
    }

    public void PlayNextStep()
    {

        if (battleLog.Count == 1)
        {
            finalResult?.Invoke(this, new ResultData(battleLog[0].ToArray()));
            return;
        }

        bool isKazamata = battleLog[0][1] == "kazamata";
        string command = battleLog[0][2];

        if (isKazamata)
        {
            UICardElement card = getCard(enemyDeck, battleLog[0][3]);

            switch (command)
            {
                case "kijatszik":
                    MoveCard(card, enemyDeckControl, enemyCardControl);
                    break;
                case "tamad":
                    UpdateCardHealth(getCard(playerDeck, battleLog[0][5]), battleLog[0][6]);
                    break;
            }
        }
        else
        {
            UICardElement card = getCard(playerDeck, battleLog[0][3]);

            switch (command)
            {
                case "kijatszik":
                    MoveCard(card, playerDeckControl, playerCardControl);
                    break;
                case "tamad":
                    UpdateCardHealth(getCard(enemyDeck, battleLog[0][5]), battleLog[0][6]);
                    break;
            }
        }

        battleLog.RemoveAt(0);
    }

    private UICardElement getCard(UICardElement[] array, string name)
    {
        return array.Where(x => x.card.Name == name).First();
    }

    private void MoveCard(UICardElement card, StackPanel from, StackPanel to)
    {
        from.Children.Remove(from.Children.Where(x => x.Name == card.card.Name).First());
        to.Children.Add(card.GetCardVisual());
    }

    private void UpdateCardHealth(UICardElement toCard, string newValue)
    {
        try
        {
            AudioManager.PlaySoundEffect(SoundEffectTypes.attack);

            if (int.Parse(newValue) > 0)
            {
                toCard.EditHealth(newValue);
            }
            else
            {
                toCard.GetCardVisual().IsVisible = false;

                if (toCard.card.isKazamata)
                {
                    enemyCardControl.Children.Remove(enemyCardControl.Children.Where(x => x.Name == toCard.card.Name).First());
                }
                else
                {
                    playerCardControl.Children.Remove(playerCardControl.Children.Where(x => x.Name == toCard.card.Name).First());
                }

                AudioManager.PlaySoundEffect(SoundEffectTypes.death);
            }
        } catch(Exception)
        {
            Console.WriteLine("Error while removing card.");
        }

    }
}