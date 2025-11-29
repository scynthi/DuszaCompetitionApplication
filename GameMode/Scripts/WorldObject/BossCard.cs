using Godot;
using System;

public partial class BossCard : Card
{
    public Card BaseCard { get; set; }
    public BossDouble Doubled;
    public BossCard(){}

    public BossCard(string name, int baseDamage, int health, CardElements cardElements, string cardIcon)
    {
        
        Name        = name;
        Health      = health;
        Icon        = cardIcon;
        BaseDamage  = baseDamage;
        Damage      = baseDamage;
        CardElement = cardElements;
        buffHandler = new BuffHandler();
        buffHandler.BindCard(this);
    }

    public BossCard(BossCard other)
    {
        Name          = other.Name;
        Health        = other.Health;
        Icon          = other.Icon;
        BaseDamage    = other.BaseDamage;
        Damage        = other.BaseDamage;
        CardElement   = other.CardElement;
        BaseCard      = other.BaseCard;
        Doubled       = other.Doubled;
        DamageChanged = other.DamageChanged;
        HealthChanged = other.HealthChanged;
        Icon          = other.Icon;
        buffHandler   = new BuffHandler();
        buffHandler.BindCard(this);
    }

    public BossCard(Card other, string addedName, BossDouble bossDouble) : base(other)
    {
        BaseCard = other;
        Name = addedName + " " + Name;
        Doubled = bossDouble;
		if (bossDouble == BossDouble.ATTACK)
			Damage *= 2;
		else
			Health *= 2;
    }
}
