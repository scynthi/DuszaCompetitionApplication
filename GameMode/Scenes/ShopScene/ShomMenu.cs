using Godot;
using System;

public partial class ShomMenu : Control
{
    public void GiveItemToPlayer(ItemType type)
    {

        IItem item = Items.CreateItemFromType(type);
        if (Global.gameManager.saverLoader.currSaveFile.player.Money < item.Price) return;

        Global.gameManager.saverLoader.currSaveFile.player.Money -= item.Price;
        Global.gameManager.saverLoader.currSaveFile.player.ItemList.Add(item);
    
        GD.Print(Global.gameManager.saverLoader.currSaveFile.player.Money);

    }
}
