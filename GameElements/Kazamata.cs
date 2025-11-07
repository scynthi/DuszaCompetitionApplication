using Avalonia.Controls.Primitives;
using DuszaCompetitionApplication.Enums;
using System;

namespace DuszaCompetitionApplication.GameElements;

public class Kazamata
{
	public KazamataTypes type { get; }
	public string name;
	public Card[] kazamataCards { get; }
	public RewardType reward { get; }

	public Kazamata(KazamataTypes type, string name, Card[] kazamataCards, RewardType reward)
	{
		this.type = type;
		this.name = name;
		this.kazamataCards = kazamataCards;
		this.reward = reward;
	}
	public string KazamataCardNames()
	{
		string rValue = "";
		foreach (Card card in kazamataCards)
		{
			rValue = rValue + card.name + ";";
		}
		return rValue;
	}
}
