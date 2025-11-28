using Godot;
using System;
using System.Data.Common;
using System.Linq;

public partial class BossEditor : HBoxContainer
{

  [Export] public Control BossCardHolder;
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

    Utility.AddUiBossCardsUnderContainer(editor.gameMasterData.WorldCards, BossCardHolder);
  }
}
