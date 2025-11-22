using Godot;
using System;

public partial class KazamataEditor : HBoxContainer
{
    [Export] UIKazamata kazamata;

    public void ChangeName(string text)
    {
        if (text.Replace(" ", "") == "") return;

        kazamata.EditName(text);
    }

    public void ChangeType(int index)
    {
        kazamata.EditType((DungeonTypes)index);
    }
}
