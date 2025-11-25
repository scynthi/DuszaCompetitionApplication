using System.Collections.Generic;
using Godot;
using Microsoft.VisualBasic;

public class GameMasterData
{
    public List<Card> WorldCards = new();
    public List<Card> PlayerDeck = new();
    public List<Card> PlayerCollection = new();
    public List<Dungeon> Dungeons = new();


    public delegate void CardDataChangedHandler();
    public delegate void DungeonDataChangedHandler();
	public event CardDataChangedHandler CardDataChanged;
	public event DungeonDataChangedHandler DungeonDataChanged;


    public void AddCardToWorldCards(Card card)
    {
        WorldCards.Add(card);
        CardDataChanged.Invoke();
    }

    public void AddCardToPlayerDeck(Card card)
    {
        if (!WorldCards.Contains(card)) WorldCards.Add(card);
        PlayerDeck.Add(card);
        CardDataChanged.Invoke();
    }

    public void AddCardToPlayerCollection(Card card)
    {
        if (!WorldCards.Contains(card)) WorldCards.Add(card);
        PlayerDeck.Add(card);
        CardDataChanged.Invoke();
    }

    public void RemoveCardFromWorldCards(Card card)
    {
        try
        {
            foreach (List<Card> currentCardList in new List<Card>[]{WorldCards, PlayerDeck, PlayerCollection})
            {
                foreach (Card cardInList in currentCardList)
                {
                    if (cardInList.Name == card.Name)
                    {
                        WorldCards.RemoveAt(WorldCards.IndexOf(cardInList)); 
                    }
                }
            }

        }
        catch (System.Exception)
        {
            GD.Print($"{card} can't be removed since it is not included in list.");
        }
        CardDataChanged.Invoke();
    }

    public void RemoveCardFromPlayerCollection(Card card)
    {
        try
        {
            foreach (List<Card> currentCardList in new List<Card>[]{PlayerDeck, PlayerCollection})
            {
                foreach (Card cardInList in currentCardList)
                {
                    if (cardInList.Name == card.Name)
                    {
                        WorldCards.RemoveAt(WorldCards.IndexOf(cardInList)); 
                    }
                }
            }
        }
        catch (System.Exception)
        {
            GD.Print($"{card} can't be removed since it is not included in list.");
        }
        CardDataChanged.Invoke();
    }

    public void RemoveCardFromPlayerDeck(Card card)
    {
        try
        {
            foreach (Card cardInList in PlayerDeck)
            {
                if (cardInList.Name == card.Name)
                {
                    WorldCards.RemoveAt(WorldCards.IndexOf(cardInList)); 
                }
            }
        }
        catch (System.Exception)
        {
            GD.Print($"{card} can't be removed since it is not included in list.");
        }
        CardDataChanged.Invoke();
    }

    public void AddDungeonToDungeonList(Dungeon dungeon)
    {
        Dungeons.Add(dungeon);
        DungeonDataChanged.Invoke();
    }

    public void RemoveDungeonFromDungeonList(Dungeon dungeon)
    {
        try
        {
            foreach (Dungeon dungeonInList in Dungeons)
            {
                if (dungeonInList.Name == dungeon.Name)
                {
                    Dungeons.RemoveAt(Dungeons.IndexOf(dungeonInList)); 
                }
            }
        }
        catch (System.Exception)
        {
            GD.Print($"{dungeon} can't be removed since it is not included in list.");
        }
        
        DungeonDataChanged.Invoke();
    }
}