using System;
using System.Collections.Generic;

namespace DuszaCompetitionApplication.Enums;

public enum CardElement
{
    fold,
    levego,
    viz,
    tuz,
}

public static class ElementRules
{
    private static readonly Dictionary<CardElement, CardElement[]> WeakTo = new()
    {
        { CardElement.levego, new[] { CardElement.tuz }},
        { CardElement.viz, new[] { CardElement.fold }},
        { CardElement.fold, new[] { CardElement.viz }},
        { CardElement.tuz, new[] { CardElement.levego }}
    };

    public static CardElement[] GetWeaknesses(CardElement e) => WeakTo[e];

    private static readonly Dictionary<CardElement, CardElement[]> StrongAgainst = new()
    {
        { CardElement.levego, new[] { CardElement.fold, CardElement.viz }},
        { CardElement.viz, new[] { CardElement.levego, CardElement.tuz }},
        { CardElement.fold, new[] { CardElement.levego, CardElement.tuz }},
        { CardElement.tuz, new[] { CardElement.fold, CardElement.viz }}
    };
    public static CardElement[] GetStrenghts(CardElement e) => StrongAgainst[e];
}