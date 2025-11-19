using Godot;
using System;

public interface IBuff
{
	public int EndingRound { get; }
	public int AlterDamageOut(int damage);
	public int AlterDamageIn(int damage);
}
