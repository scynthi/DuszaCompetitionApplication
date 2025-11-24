using Godot;
using System;

public partial class BaseItem : IItem
{
    public string Name {get;}

    public string Description {get;}

    public IBuff Buff {get;}

    public void ApplyDungeonBuff(Card card, int round){}

    public void ApplyPlayerBuff(Card card, int round){}

}
