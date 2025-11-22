using Godot;
using System;

public partial class PiciMenü : Control
{
	[Export] public UICard card;
	public string option;

	public delegate void ClickedEventHandler(PiciMenü option);
	public event ClickedEventHandler Clicked;

	public void ButtonPressed(string option)
    {
		this.option = option;
        Clicked.Invoke(this);
		QueueFree();
    }
}
