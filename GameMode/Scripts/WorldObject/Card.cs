using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Card : Node, IWorldObject
{
	public new string Name { protected set; get; }
    public int Health { protected set; get; }
    public int BaseDamage { protected set; get; }
    public int Damage { protected set; get; }
    public CardElements CardElement { private set; get; }
    public BuffHandler buffHandler { private set; get; }

    public Card(string name, int baseDamage, int health, CardElements cardElements)
    {
        Name        = name;
        Health      = health;
        BaseDamage  = baseDamage;
        Damage      = baseDamage;
        CardElement = cardElements;
        buffHandler = new BuffHandler(this);
    }
	
	public Card(Card other)
    {
        Name        = other.Name;
        Health      = other.Health;
        BaseDamage  = other.BaseDamage;
        Damage      = other.BaseDamage;
        CardElement = other.CardElement;
        buffHandler = new BuffHandler(this);
    }

    public void ModifyBaseDamage(int value)
    {
        BaseDamage = Math.Clamp(BaseDamage + value, 0, 100); 
    }
    public void ModifyHealth(int value)
    {
        Health = Math.Clamp(Health + value, 0, 100); 
    }
    public void SetBaseDamage(int value)
    {
        BaseDamage = Math.Clamp(value, 0, 100); 
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
        int calculatedDamage = ElementRules.CalculateDamage(Damage, CardElement, card.CardElement);
        card.ApplyDamage(calculatedDamage);
        return calculatedDamage;
    }
    public void ApplyDamage(int value)
    {
        Health -= value; 
        Health = Math.Clamp(Health, 0, 100);
    }
}
