using Avalonia.Controls.Primitives;
using DuszaCompetitionApplication.Enums;
using System;

namespace DuszaCompetitionApplication.GameElements;

public class Kazamata
{
    public KazamataTypes type { get; }
    public string Name { get; }
    public Card[] KazamataCards { get; }
    public RewardType reward { get; }

    public Kazamata() { Name = ""; KazamataCards = new Card[] { }; }

    public Kazamata(KazamataTypes type, string name, Card[] kazamataCards, RewardType reward)
    {
        this.type = type;
        this.Name = name;
        this.KazamataCards = kazamataCards;
        this.reward = reward;
    }

    public string KazamataCardNames()
    {
        string rValue = "";
        foreach (Card card in KazamataCards)
        {
            rValue = rValue + card.Name + ",";
        }
        rValue = rValue.Substring(0, rValue.Length - 1);
        if (type != KazamataTypes.egyszeru)
        {
            int i = rValue.LastIndexOf(',');
            if (i >= 0) rValue = rValue.Remove(i, 1).Insert(i, ";");
        }
        return rValue;
    }

    public override string? ToString()
    {
        return Name;
    }
}
