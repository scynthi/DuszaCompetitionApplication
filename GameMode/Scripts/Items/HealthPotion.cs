using Godot;
using System;
using System.Threading.Tasks;

public class HealthPotion : IItem
{
    
    public string Name { private set; get; } = "Health Potion";
	public string Description { private set; get; } = "HELLO WIRLAC";
	public IBuff Buff { private set; get; }

	public void ApplyPlayerBuff(Card card, int round)
    {
        card.ModifyHealth(100);
    }
	public void ApplyDungeonBuff(Card card, int round) { }
}
