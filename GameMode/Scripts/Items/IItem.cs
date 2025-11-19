using Godot;
using System;
public interface IItem
{
	public string ItemName { get; }
	[Export] public Texture2D texture { get; }
	public void ApplyPlayerBuff(Card card);
	public void ApplyDungeonBuff(Card card);
}
