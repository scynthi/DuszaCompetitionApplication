using Godot;
using System;

public partial class Card : IWorldObject
{
    public string Name { set; get; }
    public int Health { set; get; }
    public int BaseDamage { set; get; }
    public int Damage { set; get; }
    public bool DamageChanged { set; get; }
    public bool HealthChanged { set; get; }
    public CardElements CardElement { set; get; }
    public string Icon { set; get; }
    public BuffHandler buffHandler { set; get; }

    public Card() { }

    public Card(string name, int baseDamage, int health, CardElements cardElements, string cardIcon)
    {

        Name = name;
        Health = health;
        Icon = cardIcon;
        BaseDamage = baseDamage;
        Damage = baseDamage;
        CardElement = cardElements;
        buffHandler = new BuffHandler();
        buffHandler.BindCard(this);
    }

    public Card(string name, int baseDamage, int health, CardElements cardElements)
    {
        Name = name;
        Health = health;
        BaseDamage = baseDamage;
        Damage = baseDamage;
        CardElement = cardElements;
        buffHandler = new BuffHandler();
        buffHandler.BindCard(this);
    }

    public Card(Card other)
    {
        Name = other.Name;
        Health = other.Health;
        Icon = other.Icon;
        BaseDamage = other.BaseDamage;
        Damage = other.BaseDamage;
        CardElement = other.CardElement;
        DamageChanged = other.DamageChanged;
        HealthChanged = other.HealthChanged;
        Icon = other.Icon;
        buffHandler = new BuffHandler();
        buffHandler.BindCard(this);
    }

    public void ModifyBaseDamage(int value)
    {
        BaseDamage = Math.Clamp(BaseDamage + value, 0, 100);
        DamageChanged = true;
    }
    public void ModifyHealth(int value)
    {
        Health = Math.Clamp(Health + value, 0, 100);
        HealthChanged = true;
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
    public int Attack(Card card, bool PlayerAttacked)
    {
        Random rnd = new Random();
        double difficultyModifier;
        int damage = BaseDamage;

        if (PlayerAttacked)
        {
            difficultyModifier = 1 + rnd.NextDouble() * ((double)Global.gameManager.saverLoader.currSaveFile.gameDifficulty / 10);
        }
        else
        {
            difficultyModifier = 1 - rnd.NextDouble() * ((double)Global.gameManager.saverLoader.currSaveFile.gameDifficulty / 20);
        }
        damage = Convert.ToInt32(Math.Round(damage * difficultyModifier));
        int calculatedDamage = ElementRules.CalculateDamage(damage, CardElement, card.CardElement);
        buffHandler.CalculateDamage(calculatedDamage);
        return card.ApplyDamage();
    }
    public int ApplyDamage()
    {
        int damage = Convert.ToInt32(Math.Round(Damage * buffHandler.CalculateDamageTakenMultiplier()));
        
        Health -= damage;
        Health = Math.Clamp(Health, 0, 100);
        return damage;
    }
}
