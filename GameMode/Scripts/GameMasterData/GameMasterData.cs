using System.Collections.Generic;
using Godot;

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
            WorldCards.Remove(card); 
            PlayerDeck.Remove(card);
            PlayerCollection.Remove(card);
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
            PlayerDeck.Remove(card);
            PlayerCollection.Remove(card);
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
            PlayerDeck.Remove(card);
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
            Dungeons.Remove(dungeon);
        }
        catch (System.Exception)
        {
            GD.Print($"{dungeon} can't be removed since it is not included in list.");
        }
        
        DungeonDataChanged.Invoke();
    }
}