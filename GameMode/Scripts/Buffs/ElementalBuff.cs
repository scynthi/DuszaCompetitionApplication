using Godot;
using System;

public partial class ElementalBuff : IBuff
{
	public string Name { private set; get; }
	public string Description { private set; get; }
	public int LastsFor { private set; get; } = 1;
	public int EndingRound { private set; get; }
	public int ExtraDamage { private set; get; } = 0;
	public float DamageMultiplier { private set; get; } = 1.0f;
	public float DamageTakenMultiplier { private set; get; } = 0.5f;

	public ElementalBuff(int round)
    {
        EndingRound = round + LastsFor;
    }
}
