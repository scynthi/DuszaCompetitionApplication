using System;
using System.Collections.Generic;

namespace DuszaCompetitionApplication.GameElements;

public class Player
{
    public List<Card> collection { get; } = new List<Card>();
    public List<Card> pakli { get; } = new List<Card>();
    public void ClearCollection()
    {
        collection.Clear();
    }
    public void ClearPakli()
    {
        pakli.Clear();
    }

    public void AddToCollection(Card card)
    {
        collection.Add(card);
    }
    public void AddToCollection(Card[] cards)
    {
        foreach (Card card in cards) pakli.Add(card);
    }
    public void AddToPakli(Card card)
    {
        collection.Add(card);
    }
    public void AddToPakli(Card[] cards)
    {
        foreach (Card card in cards) pakli.Add(card);
    }
    public void PrintCollection()
    {
        foreach (Card card in collection)
        {
            Console.WriteLine(card.name);
        }
    }
    public void IncreaseAttack(int index)
    {
        pakli[index].IncreaseAttack();
    }
    public void IncreaseHealth(int index)
    {
        pakli[index].IncreaseHealth();
    }

}