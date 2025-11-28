using Godot;
using System;

public partial class WorldCardsSubmenu : Control
{
    [Export] GridContainer cardContainer;

    public void ReloadWorldCards()
    {
        if (Global.gameManager.saverLoader.currSaveFile.WorldCards.Count > 0)
        {
            Utility.AddUiSimpleCardsUnderContainer(Global.gameManager.saverLoader.currSaveFile.WorldCards, cardContainer);
            Utility.AddUiBossCardsUnderContainer(Global.gameManager.saverLoader.currSaveFile.WorldCards, cardContainer, false);        

        }
    }
}
