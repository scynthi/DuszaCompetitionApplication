using Godot;
using System;

public partial class UiAudioController : Node
{
    [Export] public AudioBank audioBank;
    [Export] public AudioStreamPlayer sfxAudioPlayer;
    [Export] public AudioStreamPlayer envAudioPlayer;

    [Export] public AudioStreamPlayer musicAudioPlayer;



    public void PlaySFX(AudioStream stream, bool randomizePitch = true)
    {
        float randomPitchScale = randomizePitch ? (float)GD.RandRange(-0.2f, 0.2f) : 0f;
        sfxAudioPlayer.PitchScale = 1.0f + randomPitchScale;
        
        sfxAudioPlayer.Stream = stream;
        sfxAudioPlayer.Play();
    }
}
