using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DeckSubmenu : Control
{
    [Export] Inventory playerCollection;
    [Export] Inventory playerDeck;

    [Export] Label moneyLabel;
    [Export] Label xpLabel;

    [Export] Label HealthPotionAmount;
    [Export] Label GlassShieldAmount;
    [Export] Label ElementalBuffAmount;

    [Export] AnimationPlayer sideAnimator;
    [Export] AnimationPlayer transAnimator;

    [Export] MapMenu mapMenu;

    public override void _Ready()
    {
        transAnimator.AnimationFinished += _onTransitionedIn;
        playerDeck.IsPlayerDeck = true;
    }

    public void _onTransitionedIn(StringName _)
    {
        if (mapMenu.CurrentMenu == this)
        {
            sideAnimator.Play("PopItemBar");        
        }
    }

    public void ReloadCards()
    {
        ReloadXPAndMoneyLabel();
        ReloadItems();
        sideAnimator.Play("RESET");

        List<Card> collectionLoadList = [];
        List<Card> deckLoadList = [];

        foreach(Card card in Global.gameManager.saverLoader.currSaveFile.player.Collection)
        {
            if (Global.gameManager.saverLoader.currSaveFile.player.Deck.Contains(card)){
                
                deckLoadList.Add(card);
            }
            else
            {
                collectionLoadList.Add(card);
            }
        }
        playerDeck.AmountOfCols = Convert.ToInt32(Math.Ceiling((double)Global.gameManager.saverLoader.currSaveFile.player.Collection.Count / 2));
        playerDeck.RemakePanelItems(Collection: Global.gameManager.saverLoader.currSaveFile.player.Deck);
        playerCollection.RemakePanelItems(Collection: Global.gameManager.saverLoader.currSaveFile.player.Collection);
    }

    public void ReloadXPAndMoneyLabel()
    {
        moneyLabel.Text = Convert.ToString(Global.gameManager.saverLoader.currSaveFile.player.Money);
        xpLabel.Text = Convert.ToString(Global.gameManager.saverLoader.currSaveFile.player.Xp);
    }

    public void ReloadItems()
    {
        HealthPotionAmount.Text = "X" + Convert.ToString(Global.gameManager.saverLoader.currSaveFile.player.ReturnItemAmount(ItemType.HealthPotion));
        GlassShieldAmount.Text = "X" + Convert.ToString(Global.gameManager.saverLoader.currSaveFile.player.ReturnItemAmount(ItemType.GlassShield));
        ElementalBuffAmount.Text = "X" + Convert.ToString(Global.gameManager.saverLoader.currSaveFile.player.ReturnItemAmount(ItemType.ElementalBuff));

    }
}
