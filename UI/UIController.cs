using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using DuszaCompetitionApplication.Audio;

namespace DuszaCompetitionApplication.UIController;

static class UIController
{
    public static void ApplySFXToButtons(Button[] buttons)
    {
        foreach (Button button in buttons)
        {
            button.Click += Button_Click;
        }
            
    }

    public static void Button_Click(object? sender, RoutedEventArgs e)
    {
        AudioManager.PlaySoundEffect(SoundEffectTypes.click);
    }
}