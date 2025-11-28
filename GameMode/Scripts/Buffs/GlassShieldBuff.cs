using Godot;
using System;

public partial class GlassShieldBuff : IBuff
{
	public string Name { private set; get; }
	public string Description { private set; get; }
	public int LastsFor { private set; get; } = 1;
	public int EndingRound { private set; get; }
	public int ExtraDamage { private set; get; }
	public float DamageMultiplier { private set; get; }
	public float DamageTakenMultiplier { private set; get; } = 0.0f;

	public GlassShieldBuff(int round)
    {
        EndingRound = round + LastsFor + 1;
    }
}
