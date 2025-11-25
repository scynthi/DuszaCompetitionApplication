using Godot;
using System;

public partial class PiciMenü : Control
{
	private Control _card;
	public Control Card
    {
        get {return _card;}
		set
        {
			_card = value;

			SetUpButtons();
        }
    }
	public string option;

	public delegate void ClickedEventHandler(PiciMenü option);
	public event ClickedEventHandler Clicked;

	public void ButtonPressed(string option)
    {
		this.option = option;
        Clicked.Invoke(this);
		QueueFree();
    }

	private void SetUpButtons()
    {
        if (Card is UICard)
        {
            
        }
    }
}
