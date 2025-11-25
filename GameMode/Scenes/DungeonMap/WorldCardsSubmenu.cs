using Godot;
using System;

public partial class WorldCardsSubmenu : Control
{
    [Export] GridContainer cardContainer;

    public void ReloadWorldCards()
    {
        Utility.AddUiCardsUnderContainer(Global.gameManager.saverLoader.currSaveFile.WorldCards, cardContainer);
    }
}
