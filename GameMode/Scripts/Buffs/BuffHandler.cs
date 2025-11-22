using Godot;
using System;
using System.Collections.Generic;

public class BuffHandler
{
	private List<IBuff> CurrentBuffs = new List<IBuff>();
	private Card OwnerCard;

	public BuffHandler(Card card)
    {
        OwnerCard = card;
    }

	public void ClearBuffs(int round)
    {
        for (int i = 0; i < CurrentBuffs.Count; i++)
        {
            if (CurrentBuffs[i].EndingRound >= round) CurrentBuffs.RemoveAt(i);
        }
    }

	public void AddBuff(IBuff buff)
    {
        CurrentBuffs.Add(buff);
    }

	public void CalculateDamage()
    {
		int damage;
		int extraDamage = 0;
		double damageMultiplier = 1.0;
		foreach (IBuff buff in CurrentBuffs)
        {
            extraDamage += buff.ExtraDamage;
			damageMultiplier += buff.DamageMultiplier;
        }
		damage = Convert.ToInt32(Math.Floor((OwnerCard.BaseDamage + extraDamage) * damageMultiplier));
        OwnerCard.SetDamage(damage);
    }

	public float CalculateDamageTakenMultiplier()
    {
        GD.Print("BRuh: " + CurrentBuffs.Count);
		float damageTaken = 1.0f;
		foreach (IBuff buff in CurrentBuffs)
        {
            damageTaken *= buff.DamageTakenMultiplier;
        }
        return damageTaken;
    }
	
}
