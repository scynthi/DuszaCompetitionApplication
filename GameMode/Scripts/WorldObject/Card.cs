using Godot;
using System;

public partial class Card : IWorldObject
{
	public string Name { set; get; }
    public int Health { set; get; }
    public int BaseDamage { set; get; }
    public int Damage { set; get; }
    public CardElements CardElement { set; get; }
    public Texture2D Icon { set; get; }
    public BuffHandler buffHandler { set; get; }

    public Card(){}

    public Card(string name, int baseDamage, int health, CardElements cardElements, Texture2D cardIcon)
    {
        
        Name        = name;
        Health      = health;
        Icon        = cardIcon;
        BaseDamage  = baseDamage;
        Damage      = baseDamage;
        CardElement = cardElements;
        buffHandler = new BuffHandler(this);
    }

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
        buffHandler.CalculateDamage();
        int calculatedDamage = ElementRules.CalculateDamage(Damage, CardElement, card.CardElement);
        card.ApplyDamage(calculatedDamage);
        return calculatedDamage;
    }
    public void ApplyDamage(int value)
    {
        Health -= Convert.ToInt32(Math.Round(value * buffHandler.CalculateDamageTakenMultiplier())); 
        Health = Math.Clamp(Health, 0, 100);
    }
}
