using Godot;
using System;

public partial class GlassShieldItem : IItem
{
	public string Name { private set; get; } = "Glass Shield";
	public string Description { private set; get; } = "I don't know";
	public IBuff Buff { get; }
	public void ApplyPlayerBuff(Card card, int round)
    {
        card.buffHandler.AddBuff(new GlassShieldBuff(round));
    }
	public void ApplyDungeonBuff(Card card, int round) { }
}
