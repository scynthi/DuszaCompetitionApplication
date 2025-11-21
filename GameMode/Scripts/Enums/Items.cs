using Godot;
using System;

public enum ItemType
{
	HealthPotion,
}

public static class Items
{
	public static IItem CreateItemFromType(ItemType type)
	{
		return type switch
		{
			ItemType.HealthPotion => new HealthPotion(),
			_ => null
		};
	}
}
