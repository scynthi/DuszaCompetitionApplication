using Godot;
using System;

public partial class DeckSubmenu : Control
{
    [Export] GridContainer cardContainer;

    public void ReloadWorldCards()
    {
        Utility.AddUiCardsUnderContainer(Global.gameManager.saverLoader.currSaveFile.player.Collection, cardContainer);        
    }
}
