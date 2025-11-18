using Godot;
using System;

public partial class Card : Node, IWorldObject
{
	public new string Name { get; private set; }
	
	public Card(Card other)
    {
        Name = other.Name;
    }
}
