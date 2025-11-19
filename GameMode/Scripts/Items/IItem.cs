using Godot;
using System;
public interface IItem
{
	public string Name { get; }
	public string Description { get; }
	public IBuff Buff { get; }
	public void ApplyPlayerBuff(Card card);
	public void ApplyDungeonBuff(Card card);
}
