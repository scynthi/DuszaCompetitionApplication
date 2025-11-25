using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

public partial class SaveFileResources : Resource
{
    [Export] public string name;
    [Export] public bool isStarted = false;

    // // [Export] public Player player;
    // // [Export] public Godot.Collections.Array<Card> worldCards;
    // // [Export] public Godot.Collections.Array<Dungeon> dungeons;

    // [Export] public int gameDifficulty = 0;

    // public SaveFileResource() { }

    // public SaveFileResource(string name, Player player, List<Card> worldCards, List<Dungeon> dungeons)
    // {
    //     this.name = name;
    //     this.player = player;
    //     this.worldCards = new Godot.Collections.Array<Card>(worldCards);
    //     this.dungeons = new Godot.Collections.Array<Dungeon>(dungeons);
    // }
}
