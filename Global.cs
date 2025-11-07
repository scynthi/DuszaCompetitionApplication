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

    private static WaveOutEvent outputDevice = new();

    public static void PlayAudio(string path)
    {
        outputDevice.Init(new AudioFileReader(path));
        outputDevice.Play();
    }


    private static LoopStream? loopStream;
    private static WaveOutEvent? waveOut;
    private static string? lastLoopedAudioPath;

    public static void PlayAndLoopAudio(string path)
    {
        if (lastLoopedAudioPath == path) return;

        if (waveOut != null)
        {
            loopStream?.Dispose();
            waveOut?.Dispose();
        }

        loopStream = new(new AudioFileReader(path));
        waveOut = new();
        lastLoopedAudioPath = path;

        waveOut.Init(loopStream);
        waveOut.Play();
    }
}