using Godot;
using Godot.Collections;
using System;

public partial class AudioBank : Node
{
    [Export] Array<AudioStream> clickSounds;
    [Export] Array<AudioStream> hoverSounds;
    [Export] Array<AudioStream> deathSounds;
    [Export] Array<AudioStream> drawSounds;
    [Export] Array<AudioStream> attackSounds;
    [Export] Array<AudioStream> levelupSounds;
    [Export] Array<AudioStream> winSounds;
    [Export] Array<AudioStream> loseSounds;

}
