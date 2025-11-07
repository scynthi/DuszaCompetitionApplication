using Avalonia.Controls.Primitives;
using DuszaCompetitionApplication.Enums;
using System;

namespace DuszaCompetitionApplication.GameElements;

public class Kazamata
{
	public KazamataTypes type { get; }
	public string name;
	public Card[] kazamataCards { get; }

	public Kazamata(KazamataTypes type, string name, Card[] kazamataCards)
	{
		this.type = type;
		this.name = name;
		this.kazamataCards = kazamataCards;
	}
}
