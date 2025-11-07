using DuszaCompetitionApplication.Enums;
using System;
using System.Runtime.ExceptionServices;

namespace DuszaCompetitionApplication.GameElements;

public class Card
{
	public string name { get; }
	public int health { get; }
	public int attack { get; }
	public CardElement type { get; }

	public Card(string name, int health, int attack, CardElement type)
	{
		this.name = name;
		this.health = health;
		this.attack = attack;
		this.type = type;
	}
}
