using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Player : Resource
{
	[Export] public int Xp { private set; get; }
	[Export] public int Money { private set; get; }
	[Export] public Godot.Collections.Array<Card> Collection { private set; get; }
	[Export] public Godot.Collections.Array<Card> Deck { private set; get; }
    public List<IItem> ItemList { private set; get; }

    public Player(){}

	public Player(int xp, int money, List<Card> collection, List<Card> deck)
    {
        Xp         = xp;
        Money      = money;
        Collection = new Godot.Collections.Array<Card>(collection);;
        Deck       = new Godot.Collections.Array<Card>(deck);
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
