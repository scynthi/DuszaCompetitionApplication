using Godot;
using System;

public partial class DeckSubmenu : Control
{
    [Export] GridContainer cardContainer;
    [Export] PackedScene UICard;
    [Export] PackedScene UIBossCard;

    public void ReloadWorldCards()
    {
        foreach (Node child in cardContainer.GetChildren())
        {
            child.QueueFree();
        }

        foreach(Card card in Global.gameManager.saverLoader.currSaveFile.player.Collection)
        {
            if (card is BossCard)
            {
                UIBossCard newUiCard = UIBossCard.Instantiate<UIBossCard>();
                newUiCard.EditAllCardInformation((BossCard)card);
                cardContainer.AddChild(newUiCard);

            } else
            {
                UICard newUiCard = UICard.Instantiate<UICard>();
                newUiCard.EditAllCardInformation(card);
                cardContainer.AddChild(newUiCard);

            }
        }
    }
}
