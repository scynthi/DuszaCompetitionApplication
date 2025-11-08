using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.VisualTree;
using DuszaCompetitionApplication.Views;
using NAudio.Wave;

static class Global
{
    public static MainWindow? mainWindow;

    public static ContentControl contentControl = new();

    public static T? getElementByName<T>(string name) where T : Control
    {
        return mainWindow?.GetVisualDescendants().OfType<T>().FirstOrDefault(x => x.Name == name);
    }

    private static WaveOutEvent simpleOutputDevice = new();

    public static void PlayAudio(string path)
    {
        simpleOutputDevice.Init(new AudioFileReader(path));
        simpleOutputDevice.Play();
    }


    private static LoopStream? loopStream;
    private static WaveOutEvent? musicOutputDevice;
    private static string? lastLoopedAudioPath;

    private static float musicVolume = 0.5f;

    public static float? MusicVolume
    {
        get { return musicVolume; }
        set
        {
            if (value != null) musicVolume = (float)value;
            if (musicOutputDevice != null) musicOutputDevice.Volume = musicVolume;
        }
    }

    public static void PlayAndLoopAudio(string path)
    {
        if (lastLoopedAudioPath == path) return;

        if (musicOutputDevice != null)
        {
            loopStream?.Dispose();
            musicOutputDevice?.Dispose();
        }

        loopStream = new(new AudioFileReader(path));
        musicOutputDevice = new();
        lastLoopedAudioPath = path;

        musicOutputDevice.Init(loopStream);
        musicOutputDevice.Play();
    }
}