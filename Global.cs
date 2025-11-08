using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.VisualTree;
using DuszaCompetitionApplication.GameElements;
using DuszaCompetitionApplication.UIResources;
using DuszaCompetitionApplication.Views;
using NAudio.Wave;

static class Global
{
    public static MainWindow? mainWindow;

    public static ContentControl contentControl = new();
    public static List<UICardElement> cardsList = new();

    public static T? getElementByName<T>(string name) where T : Control
    {
        return mainWindow?.GetVisualDescendants().OfType<T>().FirstOrDefault(x => x.Name == name);
    }
}