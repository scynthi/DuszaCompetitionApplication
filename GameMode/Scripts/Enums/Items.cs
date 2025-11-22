using Godot;
using System;

public enum ItemType
{
	HealthPotion,
	GlassShield,
	ElementalBuff,
}

public static class Items
{
	public static IItem CreateItemFromType(ItemType type)
	{
		return type switch
		{
			ItemType.HealthPotion => new HealthPotion(),
			ItemType.GlassShield => new GlassShieldItem(),
			ItemType.ElementalBuff => new ElementalBuffItem(),
			_ => null
		};
	}
}
