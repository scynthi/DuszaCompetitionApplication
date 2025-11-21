using Godot;
using System;

public partial class DescriptionBox : Control
{
	
	public static DescriptionBox Instance;

	private Label NameLabel;
	private Label DescriptionLabel;

    public override void _Ready()
    {
        Instance = this;
		NameLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/Name");
		DescriptionLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/Descripiton");

		NameLabel.Text = "APPLE";

		// Visible = false;
    }

	public void ShowDescription(string name, string description, Vector2 pos)
    {
		Position = pos;
        NameLabel.Text = name;
        DescriptionLabel.Text = description;

		Visible = true;
    }

	public void HideDescription()
    {
        Visible = false;
    }
}
