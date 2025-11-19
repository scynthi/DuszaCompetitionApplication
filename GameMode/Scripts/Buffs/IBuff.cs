using Godot;
using System;

public interface IBuff
{
	public string Name { get; }
	public string Description { get; }
	public int EndingRound { get; }
	public int ExtraDamage { get; }
	public int DamageMultiplier { get; }
	public int DamageTakenMultiplier { get; }
}