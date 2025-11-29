using Godot;
using System;

public partial class ElementalBuffItem : IItem
{
	public string Name { private set; get; } = "Elem talizmán";
	public string Description { private set; get; } = "Haszánlata után a \n jelenlegi kártya /2 sebzést kap \n és *2 támadást ad 1 körig";
	public string Icon { private set; get; } = "uid://cc17bg13ixypu";
    public int Price { private set; get; } = 20;
	public int Amount { set; get; } = 0;
    public ItemType Type { private set; get; } = ItemType.ElementalBuff;
	public IBuff Buff { get; }
    public void ApplyPlayerBuff(Card card, int round)
    {
        card.buffHandler.AddBuff(new ElementalBuff(round));
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
    public ElementalBuffItem() {}

}
