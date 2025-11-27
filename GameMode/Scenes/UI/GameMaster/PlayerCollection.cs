using Godot;
using System;

public partial class PlayerCollection : HBoxContainer
{
	[Export] Control cardHolder;
	Editors editor = Global.masterEditor;

    public override void _Ready()
    {
		editor.gameMasterData.CardDataChanged += HandleDataChange;
    }

    public override void _ExitTree()
    {
		editor.gameMasterData.CardDataChanged -= HandleDataChange;
    }

	public void HandleDataChange()
    {
		if (!Visible) return;
        Utility.AddUiSimpleCardsUnderContainer(editor.gameMasterData.PlayerCollection, cardHolder);
    }
}
