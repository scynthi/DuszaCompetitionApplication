using Godot;
using System;

public partial class SettingsMenu : Control
{
    [Export] Slider sfxSlider;
    [Export] Slider musicSlider;


    public void ReloadSliders()
    {
        sfxSlider.Value = Global.gameManager.audioController.sfxAudioPlayer.VolumeDb;
        musicSlider.Value = Global.gameManager.audioController.musicAudioPlayer.VolumeDb;
    }


    public void _SFXVolumeChanged(float value)
    {
        Global.gameManager.audioController.sfxAudioPlayer.VolumeDb = value;
        Global.gameManager.audioController.envAudioPlayer.VolumeDb = value;

        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());
    }

    public void _MusicVolumeChanged(float value)
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());

        Global.gameManager.audioController.musicAudioPlayer.VolumeDb = value;
    }

    public void _CrtToggeled(bool status)
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());

        Global.gameManager.ctrShaderRect.Visible = status;
    }
}
