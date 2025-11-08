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
        collection.Add(new Card(card));
    }
    public void AddToCollection(Card[] cards)
    {
        foreach (Card card in cards) pakli.Add(new Card(card));
    }
    public void AddToPakli(Card card)
    {
        if (pakli.Count < Math.Ceiling((double)collection.Count/2)) pakli.Add(new Card(card));
    }
    public void AddToPakli(Card[] cards)
    {
        foreach (Card card in cards) if (pakli.Count < Math.Ceiling((double)collection.Count / 2)) pakli.Add(new Card(card));
    }
    public void RemoveFromPakli(int i)
    {
        pakli.RemoveAt(i);
    }
    public void PrintCollection()
    {
        foreach (Card card in collection)
        {
            Console.WriteLine(card.Name);
        }
    }
    public void IncreaseAttack(int colIndex, int pakIndex)
    {
        collection[colIndex].IncreaseAttack();
        pakli[pakIndex].IncreaseAttack();
    }
    public void IncreaseHealth(int colIndex, int pakIndex)
    {
        collection[colIndex].IncreaseHealth();
        pakli[pakIndex].IncreaseHealth();
    }

}