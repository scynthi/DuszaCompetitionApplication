using Godot;
using System;

public partial class DescriptionBox : Control
{
	
	public static DescriptionBox Instance;

	private Label NameLabel;
	private Label DescriptionLabel;
    [Export] public PanelContainer Panel;

    public override void _Ready()
    {
        Instance = this;
		NameLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/Name");
		DescriptionLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/Descripiton");

		NameLabel.Text = "APPLE";

		Visible = false;
    }

	public void ShowDescription(string name, string description, Vector2 pos, float width)
    {
        NameLabel.Text = name;
        DescriptionLabel.Text = description;

		Visible = true;
		Position = pos + new Vector2(-Panel.Size.X / 2 + width / 2, -Panel.Size.Y);
    }

	public void HideDescription()
    {
        Visible = false;
    }
}
