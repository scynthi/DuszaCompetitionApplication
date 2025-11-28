using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Godot;

public class GameMasterData
{
    public List<Card> WorldCards = new();
    public List<Card> PlayerDeck = new();
    public List<Card> PlayerCollection = new();
    public List<Dungeon> Dungeons = new();
    public string SaveName;

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
        PlayerDeck.Add(card);
        CardDataChanged.Invoke();
    }

    public void AddCardToPlayerCollection(Card card)
    {
        PlayerCollection.Add(card);
        CardDataChanged.Invoke();
    }

    public void RemoveCardFromWorldCards(Card card)
    {
        DeleteCard(card, WorldCards);
        DeleteCard(card, PlayerCollection);

        CardDataChanged.Invoke();
    }

    public void RemoveCardFromPlayerCollection(Card card)
    {
        DeleteCard(card, PlayerCollection);

        CardDataChanged.Invoke();
    }

    public void RemoveCardFromPlayerDeck(Card card)
    {
        try
        {
            foreach (Card cardInList in PlayerDeck)
            {
                if (cardInList.Name != card.Name) continue;
                
                PlayerDeck.RemoveAt(WorldCards.IndexOf(cardInList));   
            }
        }
        catch (System.Exception)
        {
            GD.Print($"DECK DELETION: {card} can't be removed since it is not included in list.");
        }

        CardDataChanged.Invoke();
    }

    public void AddDungeonToDungeonList(Dungeon dungeon)
    {
        Dungeons.Add(dungeon);
        Dungeons = Dungeons.OrderBy(x =>  x.DungeonType).ToList();
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

    public bool TestCard(Card card, List<Card> collection)
    {
        var result = collection.Where(x => x.Name == card.Name);

        if (result.Count() != 0)
        {
            GD.Print($"Card with name: {card.Name} already exists!");
            return false;
        }
        return true;
    }

    public bool TestCard(Card card)
    {
        var result = WorldCards.Where(x => x.Name == card.Name);

        if (result.Count() != 0)
        {
            GD.Print($"Card with name: {card.Name} already exists!");
            return false;
        }
        return true;
    }

    public bool TesdtDungeon(Card card)
    {
        var result = WorldCards.Where(x => x.Name == card.Name);

        if (result.Count() != 0)
        {
            GD.Print($"Card with name: {card.Name} already exists!");
            return false;
        }
        return true;
    }

    private void DeleteCard(Card card, List<Card> collection)
    {
        for (int i = collection.Count - 1; i >= 0; i--)
        {
            var currentCard = collection[i];

            if (currentCard is BossCard boss)
            {
                if (boss.BaseCard.Name == card.Name || boss.Name == card.Name)collection.RemoveAt(i);
            }
            else
            {
                if (currentCard.Name == card.Name) collection.RemoveAt(i);
            }
        }
    }
}