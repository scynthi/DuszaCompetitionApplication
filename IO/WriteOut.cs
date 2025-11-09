using DuszaCompetitionApplication.GameElements;
using DuszaCompetitionApplication.Enums;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DuszaCompetitionApplication.IO;

public static class WriteOut
{
    public static void Player(Player player, string path, string name)
    {
        path = path + @$"\{name}";
        List<string> output = new List<string>();
        foreach (Card card in player.collection)
        {
            output.Add($"gyujtemeny;{card.Name};{card.Attack};{card.Health};{card.Element}");
        }
        output.Add("");
        foreach (Card card in player.pakli)
        {
            output.Add($"pakli;{card.Name}");
        }
        File.WriteAllLines(path, output);
    }
    public static void Vilag(Card[] cards, Kazamata[] kazamatas, string path, string name)
    {
        path = path + @$"\{name}";
        List<string> output = new List<string>();

        List<string> normalCards = new List<string>();
        List<string> vezerCards = new List<string>();
        foreach (Card card in cards)
        {
            if (card.Type == CardType.sima) normalCards.Add($"kartya;{card.Name};{card.Attack};{card.Health};{card.Element}");
            else vezerCards.Add($"vezer;{card.Name};{card.Attack};{card.Health};{card.Element}");
        }

        if (normalCards.Count > 0)
        {
            output.AddRange(normalCards);
            output.Add("");
        }
        if (vezerCards.Count > 0)
        {
            output.AddRange(vezerCards);
            output.Add("");
        }

        foreach (Kazamata kazamata in kazamatas)
        {
            if (kazamata.reward != RewardType.kartya) output.Add($"kazamata;{kazamata.type.ToString()};{kazamata.Name};{kazamata.KazamataCardNames()};{kazamata.reward.ToString()}");
            else output.Add($"kazamata;{kazamata.type.ToString()};{kazamata.Name};{kazamata.KazamataCardNames()}");
        }
        File.WriteAllLines(path, output);
    }
    public static void Battle(string[] output, string path, string name)
    {
        path = path + @$"\{name}";
        File.WriteAllLines(path, output);
    }
}