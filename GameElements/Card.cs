using DuszaCompetitionApplication.Enums;
using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Xml.Linq;

namespace DuszaCompetitionApplication.GameElements;

public class Card
{
    public string Name { get; }
    public int Health { get; private set; }
    public int Attack { get; private set; }
    public CardElement Element { get; }
    public CardType Type { get; }

    public Card(string name, int attack, int health, CardElement element, CardType type)
    {
        this.Name = name;
        this.Attack = attack;
        this.Health = health;
        this.Element = element;
        this.Type = type;
    }
    public Card(Card other)
    {
        this.Name = other.Name;
        this.Attack = other.Attack;
        this.Health = other.Health;
        this.Element = other.Element;
        this.Type = other.Type;
    }

    public void Damage(int attack, CardElement element)
    {
        if (element == this.Element)
        {
            Health -= attack;
        }
        else if (ElementRules.GetWeaknesses(element).Contains(this.Element))
        {
            Health -= Convert.ToInt32(Math.Floor((double)attack / 2));
        }
        else if (ElementRules.GetStrenghts(element).Contains(this.Element))
        {
            Health -= attack * 2;
        }
        if (Health < 0) Health = 0;
    }
    public void IncreaseAttack()
    {
        Attack++;
    }
    public void IncreaseHealth()
    {
        Health += 2;
    }
}
