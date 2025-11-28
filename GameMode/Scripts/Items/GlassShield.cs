using Godot;
using System;

public partial class GlassShieldItem : IItem
{
	public string Name { private set; get; } = "Üveg pajzs";
	public string Description { private set; get; } = "Használata után az \n elenfél első ütése \n bármekkora is nem érvényes";
	public string Icon { private set; get; } = "uid://duvi63fq53dex";
    public int Price { private set; get; } = 10;
	public int Amount { set; get; } = 0;
    public ItemType Type { private set; get; } = ItemType.GlassShield;
	public IBuff Buff { get; }
    public void ApplyPlayerBuff(Card card, int round)
    {
        card.buffHandler.AddBuff(new GlassShieldBuff(round));
    }
	public void ApplyDungeonBuff(Card card, int round) { }
    public void IncreaseAmount()
    {
        Amount++;
    }
	public void DecreaseAmount()
    {
        Amount--;
    }

    public GlassShieldItem() {}
}
