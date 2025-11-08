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
            button.Click += (_,_) => {AudioManager.PlaySoundEffect(SoundEffectTypes.click);};
            button.PointerEntered += (_,_) => {AudioManager.PlaySoundEffect(SoundEffectTypes.hover);};
        }
    }
}