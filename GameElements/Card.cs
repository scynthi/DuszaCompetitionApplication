using DuszaCompetitionApplication.Enums;
using System;
using System.Runtime.ExceptionServices;

namespace DuszaCompetitionApplication.GameElements;

public class Card
{
	public string name { get; }
	public int health { get; }
	public int attack { get; }
	public CardElement element { get; }
	public CardType type { get; }

	public Card(string name, int attack, int health, CardElement element, CardType type)
	{
		this.name = name;
		this.attack = attack;
		this.health = health;
		this.element = element;
		this.type = type;
	}

	public void Damage(int attack, CardElement element)
	{
		
	}
}
