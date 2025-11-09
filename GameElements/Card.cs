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
    public bool isKazamata { get; set; }
    public bool HealthChanged { get; private set; } = false;
    public bool AttackChanged { get; private set; } = false;
    public Card() { Name = ""; }

    public Card(string name, int attack, int health, CardElement element, CardType type)
    {
        this.Name = name;
        this.Attack = attack;
        this.Health = health;
        this.Element = element;
        this.Type = type;
        Health = Math.Clamp(Health, 0, 100);
        Attack = Math.Clamp(Attack, 0, 100);
    }
    public Card(Card other)
    {
        Name = other.Name;
        Attack = Math.Clamp(other.Attack, 0, 100);
        Health = Math.Clamp(other.Health, 0, 100);
        Element = other.Element;
        Type = other.Type;
        AttackChanged = other.AttackChanged;
        HealthChanged = other.HealthChanged;
    }
    public Card Clone()
    {
        return new Card(this);
    }

    public int Damage(int attack, CardElement element)
    {
        int damageNum = 0;
        if (element == this.Element)
        {
            damageNum = attack;
        }
        else if (ElementRules.GetWeaknesses(element).Contains(this.Element))
        {
            damageNum = Convert.ToInt32(Math.Floor((double)attack / 2));
        }
        else if (ElementRules.GetStrenghts(element).Contains(this.Element))
        {
            damageNum = attack * 2;
        }
        Health -= damageNum;
        Health = Math.Clamp(Health, 0, 100);
        return damageNum;
    }
    public void IncreaseAttack()
    {
        AttackChanged = true;
        Attack++;
        Attack = Math.Clamp(Attack, 0, 100);
    }
    public void IncreaseHealth()
    {
        HealthChanged = true;
        Health += 2;
        Health = Math.Clamp(Health, 0, 100);
    }
}
