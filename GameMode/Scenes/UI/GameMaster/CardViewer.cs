using Godot;
using System;

public partial class CardViewer : HBoxContainer
{
    [Export] Control cardHolder;
	Editors editor;

    public override void _Ready()
    {
        editor = (Editors)GetParent();
		editor.gameMasterData.CardDataChanged += HandleDataChange;
    }

    public override void _ExitTree()
    {
		editor.gameMasterData.CardDataChanged -= HandleDataChange;
    }

	public void HandleDataChange()
    {
		if (!Visible) return;
        Utility.AddUiCardsUnderContainer(editor.gameMasterData.WorldCards, cardHolder);
    }

}
