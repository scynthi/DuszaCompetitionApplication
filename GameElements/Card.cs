using DuszaCompetitionApplication.Enums;
using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Xml.Linq;

namespace DuszaCompetitionApplication.GameElements;

public class Card
{
    public string name { get; }
    public int health { get; private set; }
    public int attack { get; private set; }
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
    public Card(Card other)
    {
        this.name = other.name;
        this.attack = other.attack;
        this.health = other.health;
        this.element = other.element;
        this.type = other.type;
    }

    public void Damage(int attack, CardElement element)
    {
        if (element == this.element)
        {
            health -= attack;
        }
        else if (ElementRules.GetWeaknesses(element).Contains(this.element))
        {
            health -= Convert.ToInt32(Math.Floor((double)attack / 2));
        }
        else if (ElementRules.GetStrenghts(element).Contains(this.element))
        {
            health -= attack * 2;
        }
        if (health < 0) health = 0;
    }
    public void IncreaseAttack()
    {
        attack++;
    }
    public void IncreaseHealth()
    {
        health += 2;
    }
}
