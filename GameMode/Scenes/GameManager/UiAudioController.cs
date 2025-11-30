using Godot;
using System;

public partial class UiAudioController : Node
{
    [Export] public AudioBank audioBank;
    [Export] public AudioStreamPlayer sfxAudioPlayer;
    [Export] public AudioStreamPlayer envAudioPlayer;

    [Export] public AudioStreamPlayer musicAudioPlayer;

    public void PlaySFX(AudioStream stream, bool randomizePitch = true, float setPtich = 0.0f, float rndPtichRangeMin = -0.2f, float rndPtichRangeMax = 0.2f)
    {
        float randomPitchScale = randomizePitch ? (float)GD.RandRange(rndPtichRangeMin, rndPtichRangeMax) : 0.0f;
        
        AudioStreamPlayer player = new AudioStreamPlayer();
        player.Stream = stream;
        player.PitchScale = 1.0f + randomPitchScale + setPtich;
        player.Bus = sfxAudioPlayer.Bus;
        player.VolumeDb = sfxAudioPlayer.VolumeDb;
        
        AddChild(player);
        player.Play();
        player.Finished += () => {
            player.QueueFree();
        };
    }

    public void PlayMusicAndEnvSounds(AudioStream music, AudioStream ambient)
    {
        musicAudioPlayer.Stop();
        envAudioPlayer.Stop();

        musicAudioPlayer.Stream = music;
        envAudioPlayer.Stream = ambient;

        musicAudioPlayer.Play();
        envAudioPlayer.Play();
    }
}
