using Godot;
using System;

public partial class Card : Node, IWorldObject
{
	public new string Name { protected set; get; }
    public int Health { protected set; get; }
    public int Damage { protected set; get; }
    public CardElements CardElement { private set; get; }

    public Card(string name, int health, int damage, CardElements cardElements)
    {
        Name        = name;
        Health      = health;
        Damage      = damage;
        CardElement = cardElements;
    }
	
	public Card(Card other)
    {
        Name        = other.Name;
        Health          = other.Health;
        Damage      = other.Damage;
        CardElement = other.CardElement;
    }
}
