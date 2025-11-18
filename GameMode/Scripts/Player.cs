using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Node
{
	public int Xp { private set; get; }
	public int Money { private set; get; }
	public List<Card> Collection { private set; get; }
	public List<Card> Deck { private set; get; }

	public Player(int xp, int money, List<Card> collection, List<Card> deck)
    {
        Xp         = xp;
        Money      = money;
        Collection = collection;
        Deck       = deck;
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

	public bool TryAddToDeck(string name)
    {
		foreach (Card card in Collection) if (card.Name == name) { Deck.Add(card); return true; }
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
		if (oldIndex < 0 || oldIndex >= Deck.Count) return;
		if (newIndex < 0 || newIndex >= Deck.Count) return;

        Card selectedCard = Deck[oldIndex];

		Deck.RemoveAt(oldIndex);
		Deck.Insert(newIndex, selectedCard);
    }
}
