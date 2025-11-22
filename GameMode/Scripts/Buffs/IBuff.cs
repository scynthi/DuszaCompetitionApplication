using Godot;
using System;

public interface IBuff
{
	public string Name { get; }
	public string Description { get; }
	public int LastsFor { get; }
	public int EndingRound { get; }
	public int ExtraDamage { get; }
	public float DamageMultiplier { get; }
	public float DamageTakenMultiplier { get; }
}