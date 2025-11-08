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
}