using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

public partial class SaveFileResource : Resource
{
    public string name;
    public bool isStarted = false;

    public Player player;
    public List<Card> worldCards;

    public List<Dungeon> dungeons;

    public int gameDifficulty = 0;


    public SaveFileResource(string name, Player player, List<Card> worldCards, List<Dungeon> dungeons)
    {
        this.name = name;

        this.player = player;
        this.worldCards = worldCards;
        this.dungeons = dungeons;
    }
}
