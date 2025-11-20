using Godot;
using System;

public partial class UiPlayground : Control
{
    [Export] PackedScene cardUIScene;

    public override void _Ready()
    {
        UICard uiCard = (UICard)cardUIScene.Instantiate();
        AddChild(uiCard);
        uiCard.EditAllCardInformation("res://Assets/Images/Entities/Enemies/char_wendigo.png", CardElements.EARTH, "AlberticsicskaJános", 10, 3, false, true);
    }
}
