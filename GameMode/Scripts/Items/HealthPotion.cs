using Godot;
using System;
using System.Threading.Tasks;

public class HealthPotion : IItem
{
    
    public string Name { private set; get; } = "Élet ital";
	public string Description { private set; get; } = "Használatakor megnöveli \n a jelenlegi kártya életerejét \n 5 ponttal";
    public string Icon { private set; get; } = "uid://b1y7hb6ugsa1s";
    public int Price { private set; get; } = 5;
	public int Amount { set; get; } = 0;
    public ItemType Type { private set; get; } = ItemType.HealthPotion;
	public IBuff Buff { private set; get; }
    public void ApplyPlayerBuff(Card card, int round)
    {
        card.ModifyHealth(5);
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

    public HealthPotion() { }
}
