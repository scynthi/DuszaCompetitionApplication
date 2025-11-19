using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ElementRules
{
	private static readonly Dictionary<CardElements, CardElements[]> WeakTo = new()
    {
        { CardElements.WIND, new[] { CardElements.FIRE }},
        { CardElements.WATER, new[] { CardElements.EARTH }},
        { CardElements.EARTH, new[] { CardElements.WATER }},
        { CardElements.FIRE, new[] { CardElements.WIND }}
    };

    public static CardElements[] GetWeaknesses(CardElements e) => WeakTo[e];

    private static readonly Dictionary<CardElements, CardElements[]> StrongAgainst = new()
    {
        { CardElements.WIND, new[] { CardElements.EARTH, CardElements.WATER }},
        { CardElements.WATER, new[] { CardElements.WIND, CardElements.FIRE }},
        { CardElements.EARTH, new[] { CardElements.WIND, CardElements.FIRE }},
        { CardElements.FIRE, new[] { CardElements.EARTH, CardElements.WATER }}
    };
    public static CardElements[] GetStrenghts(CardElements e) => StrongAgainst[e];

	public static int CalculateDamage(int damage, CardElements enemyCard, CardElements playerCard)
    {
		if (GetWeaknesses(playerCard).Contains(enemyCard)) 
			damage *= 2;
		else
			damage = Convert.ToInt32(Math.Floor((double)damage / 2));
        return damage;
    }
}
