using Godot;
using System;

public partial class BossCard : Card
{
    public BossCard(Card other, string addedName, BossDouble bossDouble) : base(other)
    {
        Name = addedName + " " + Name;
		if (bossDouble == BossDouble.ATTACK)
			Damage *= 2;
		else
			Health *= 2;
    }
}
