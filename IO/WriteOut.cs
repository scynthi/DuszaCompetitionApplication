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
            output.Add($"gyujtemeny;{card.name};{card.attack};{card.health};{card.element}");
        }
        output.Add("");
        foreach (Card card in player.pakli)
        {
            output.Add($"pakli;{card.name}");
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
            if (card.type == CardType.sima) normalCards.Add($"kartya;{card.name};{card.attack};{card.health};{card.element}");
            else vezerCards.Add($"vezer;{card.name};{card.attack};{card.health};{card.element}");
        }
        output.AddRange(normalCards);
        output.Add("");
        output.AddRange(vezerCards);
        output.Add("");

        foreach (Kazamata kazamata in kazamatas)
        {
            string kazCardNames = kazamata.KazamataCardNames();
            if (kazamata.reward != RewardType.kartya) output.Add($"kazamata;{kazamata.type.ToString()};{kazamata.name};{kazCardNames};{kazamata.reward.ToString()}");
            else output.Add($"kazamata;{kazamata.type.ToString()};{kazamata.name};{kazCardNames}");
        }
        File.WriteAllLines(path, output);
    }
    public static void Battle(string[] output, string path, string name)
    {
        path = path + @$"\{name}";
        File.WriteAllLines(path, output);
    }
}