using Godot;
using System;
using System.Text.Json.Serialization;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(HealthPotion), "health_potion")]
[JsonDerivedType(typeof(ElementalBuffItem), "elemental_buff")]
[JsonDerivedType(typeof(GlassShieldItem), "glass_shield")]
public interface IItem
{
	public string Name { get; }
	public string Description { get; }
	public string Icon { get; }
	public int Price { get; }
	public ItemType Type { get; }
	public IBuff Buff { get; }
	public void ApplyPlayerBuff(Card card, int round);
	public void ApplyDungeonBuff(Card card, int round);
}
