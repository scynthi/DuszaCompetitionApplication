using Godot;
using Godot.Collections;
using System;

public partial class AudioBank : Node
{
    [Export] public Array<AudioStream> hoverSounds;
    [Export] public Array<AudioStream> clickSounds;
    [Export] public Array<AudioStream> deathSounds;
    [Export] public Array<AudioStream> drawSounds;
    [Export] public Array<AudioStream> attackSounds;
    [Export] public Array<AudioStream> levelupSounds;
    [Export] public Array<AudioStream> winSounds;
    [Export] public Array<AudioStream> loseSounds;

}
