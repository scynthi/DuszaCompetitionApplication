using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player
{
    public int Xp { set; get; }
    public int Money { set; get; }
    public List<Card> Collection { set; get; }
    public List<Card> Deck { set; get; }
    public List<IItem> ItemList { set; get; }

    public Player() { }

    public Player(int xp, int money)
    {
        Xp = xp;
        Money = money;
        Collection = new List<Card>();
        Deck = new List<Card>();
        ItemList = new List<IItem>();
        SetUpCollection();
    }

    public void SetUpCollection()
    {
        foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
            AddToItemList(Items.CreateItemFromType(itemType));
    }

    public void SetCollection(List<Card> cardList)
    {
        Collection.Clear();
        foreach (Card card in cardList) Collection.Add(new Card(card));
    }

    public bool TryAddCardToCollection(Card card)
    {
        if (Collection.Contains(card)) return false;
        Collection.Add(new Card(card));
        return true;
    }

    public Card ReturnCardFromCollection(string name)
    {
        GD.Print(Collection.Count);
        foreach (Card card in Collection)
        {
            GD.Print(card.Name);
            if (card.Name == name) return card;
        }
        return null;
    }

    public bool TryAddToDeck(string name)
    {
        foreach (Card card in Collection) if (card.Name == name) { Deck.Add(card); return true; }
        return false;
    }

    public bool TryRemoveFromDeck(string name)
    {
        foreach (Card card in Deck) if (card.Name == name) { Deck.Remove(card); return true; }
        return false;
    }

    public bool TryAddToDeckAtIndex(string name, int index)
    {
        foreach (Card card in Collection) if (card.Name == name) { Deck.Insert(index, card); return true; }
        return false;
    }

    public void MoveCardToIndexInDeck(string name, int newIndex)
    {
        Card selectedCard = null;
        int oldIndex = -1;
        newIndex--;
        for (int i = 0; i < Deck.Count; i++)
            if (Deck[i].Name == name)
            {
                selectedCard = Deck[i];
                oldIndex = i;
            }

        if (oldIndex == -1 || selectedCard == null) return;

        Deck.RemoveAt(oldIndex);
        Deck.Insert(newIndex, selectedCard);
    }
    public void MoveCardToIndexInDeck(int oldIndex, int newIndex)
    {
        newIndex--;
        if (oldIndex < 0 || oldIndex >= Deck.Count) return;
        if (newIndex < 0 || newIndex >= Deck.Count) return;

        Card selectedCard = Deck[oldIndex];

        Deck.RemoveAt(oldIndex);
        Deck.Insert(newIndex, selectedCard);
    }
    public void AddToItemList(IItem item)
    {
        foreach (IItem currItem in ItemList)
            if (currItem.Name == item.Name)
            {
                currItem.IncreaseAmount();
                return;
            }
        ItemList.Add(item);
    }
    public void RemoveFromItemList(IItem item)
    {
        foreach (IItem currItem in ItemList)
            if (currItem.Name == item.Name)
            {
                currItem.DecreaseAmount();
                return;
            }
    }
    public bool IsInItemList(IItem item)
    {
        foreach (IItem currItem in ItemList)
            if (currItem.Name == item.Name)
            {
                return currItem.Amount > 0;
            }
        return false;
    }
    public int ReturnItemAmount(IItem item)
    {
        foreach (IItem currItem in ItemList)
            if (currItem.Name == item.Name)
            {
                return currItem.Amount;
            }
        return 0;
    }
}
