using Godot;
using System;

public partial class BossCard : Card
{
    public Card BaseCard { get; set; }
    public BossDouble Doubled;
    public BossCard(){}

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
