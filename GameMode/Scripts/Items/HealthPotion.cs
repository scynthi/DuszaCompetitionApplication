using Godot;
using System;
using System.Threading.Tasks;

public partial class HealthPotion : Node2D, IItem
{
    public new string Name { private set; get; }
	public string Description { private set; get; }
	public IBuff Buff { private set; get; }

	public void ApplyPlayerBuff(Card card)
    {
        
    }
	public void ApplyDungeonBuff(Card card)
    {
        
    }
}
