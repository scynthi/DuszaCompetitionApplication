using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Card : Node, IWorldObject
{
	public new string Name { protected set; get; }
    public int Health { protected set; get; }
    public int Damage { protected set; get; }
    public CardElements CardElement { private set; get; }
    public List<IBuff> BuffList { private set; get;}

    public Card(string name, int damage, int health, CardElements cardElements)
    {
        Name        = name;
        Health      = health;
        Damage      = damage;
        CardElement = cardElements;
    }
	
	public Card(Card other)
    {
        Name        = other.Name;
        Health      = other.Health;
        Damage      = other.Damage;
        CardElement = other.CardElement;
    }

    public void ModifyDamage(int value)
    {
        Damage = Math.Clamp(Damage + value, 0, 100); 
    }
    public void ModifyHealth(int value)
    {
        Health = Math.Clamp(Health + value, 0, 100); 
    }
    public void SetDamage(int value)
    {
        Damage = Math.Clamp(value, 0, 100); 
    }
    public void SetHealth(int value)
    {
        Health = Math.Clamp(value, 0, 100); 
    }
    public int Attack(Card card)
    {
        int damage = ElementRules.CalculateDamage(Damage, CardElement, card.CardElement);
        card.ApplyDamage(damage);
        return damage;
    }
    public void ApplyDamage(int value)
    {
        Health = Math.Clamp(Health - value, 0, 100); 
    }

    public void ClearBuffs(int round)
    {
        for (int i = 0; i < BuffList.Count; i++)
        {
            if (BuffList[i].EndingRound >= round) BuffList.RemoveAt(i);
        }
    }
}
