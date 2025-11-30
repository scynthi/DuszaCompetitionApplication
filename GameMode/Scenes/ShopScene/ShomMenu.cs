using Godot;
using System;

public partial class ShomMenu : Control
{
    [Export] Label moneyLabel;
    [Export] AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        UpdateMoneyText();

        Global.gameManager.audioController.PlayMusicAndEnvSounds(Global.gameManager.audioController.audioBank.ShopMusic,null);

    }

    public void GiveItemToPlayer(ItemType type)
    {
        IItem item = Items.CreateItemFromType(type);
        if (Global.gameManager.saverLoader.currSaveFile.player.Money < item.Price)
        {
            Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.buyFailSounds.PickRandom());
            animationPlayer.Play("Flare");
            return;
        }

        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.MoneySounds.PickRandom());

        Global.gameManager.saverLoader.currSaveFile.player.Money -= item.Price;
        Global.gameManager.saverLoader.currSaveFile.player.AddToItemList(item);
    
        UpdateMoneyText();
        animationPlayer.Play("Glare");
    }

    public void UpdateMoneyText()
    {
        moneyLabel.Text = Convert.ToString(Global.gameManager.saverLoader.currSaveFile.player.Money);
    }
}
