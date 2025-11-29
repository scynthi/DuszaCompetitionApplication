using Godot;
using System;

public partial class WorldCardsSubmenu : Control
{
    [Export] GridContainer cardContainer;

    public void ReloadWorldCards()
    {
        if (Global.gameManager.saverLoader.currSaveFile.WorldCards.Count > 0)
        {
            Utility.AddUiCardsUnderContainer(Global.gameManager.saverLoader.currSaveFile.WorldCards, cardContainer);
        }
    }
}
