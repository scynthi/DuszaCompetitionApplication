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
        output.Add("\n");
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
            else                            vezerCards.Add($"vezer;{card.name};{card.attack};{card.health};{card.element}");
        }
        output.AddRange(normalCards);
        output.Add("\n");
        output.AddRange(vezerCards);
        output.Add("\n");

        foreach (Kazamata kazamata in kazamatas)
        {
            output.Add($"kazamata;{kazamata.type.ToString()};{kazamata.name};{kazamata.KazamataCardNames()}"); // the last ';' is not need due to the method already having it
        }
        File.WriteAllLines(path, output);
    }
}