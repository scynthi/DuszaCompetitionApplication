using Godot;
using System;

public partial class CardEditor : HBoxContainer
{
	[Export] UICard card;

	public void ChangeName(string text)
    {
        card.EditName(text);
    }

    public void ChangeHealth(string text)
    {
        if (!int.TryParse(text, out _) || text.Replace(" ", "") == "") return;
        
        card.EditDamage(int.Parse(text));
    }

    public void ChangeDamage(string text)
    {
        if (!int.TryParse(text, out _) || text.Replace(" ", "") == "") return;

        card.EditDamage(int.Parse(text));
    }

    public void ChangeElement(int index)
    {
        card.EditElement((CardElements)index);
    }

    public void ChangeType()
    {
        card.isBoss = !card.isBoss;
        card.EditEffect();
    }

}
