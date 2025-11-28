using Godot;
using System;

public partial class DescriptionBox : Control
{
	[Export] private Label NameLabel;
	[Export] private Label DescriptionLabel;
    [Export] public Label Amount;
    [Export] public PanelContainer Panel;

    public override void _Ready()
    {
		Visible = false;
    }

	public void ShowDescription(string name, string description, int amount, Vector2 pos, float width)
    {
        NameLabel.Text = name;
        DescriptionLabel.Text = description;
        Amount.Text = amount.ToString();

		Visible = true;
		GlobalPosition = pos + new Vector2(-Panel.Size.X / 2 + width / 2, -Panel.Size.Y - 50);
    }

    public void UpdateDescription(string name, string description, int amount)
    {
        NameLabel.Text = name;
        DescriptionLabel.Text = description;
        Amount.Text = amount.ToString();
    }

	public void HideDescription()
    {
        Visible = false;
    }
}
