using Godot;
using System;
using System.Threading.Tasks;

public partial class UICard : Control
{   
    public async void EditAllCardInformation(string name, int hp, int damage, bool isEnemy = false, bool isBoss = false)
    {
        Label damageLabel = GetNode<Label>("Axe/Damage");
        Label healthLabel = GetNode<Label>("Heart/Health");  
        Label namehabel = GetNode<Label>("Name");  

        TextureRect charcaterIcon = GetNode<TextureRect>("Character");
        ColorRect cardBackground = GetNode<ColorRect>("Background");
        TextureRect effectTexture = GetNode<TextureRect>("Effect");

        namehabel.Text = name;
        healthLabel.Text = hp.ToString();
        damageLabel.Text = damage.ToString();

        string[] effectsPathList = {"res://Assets/Images/Cards/cards_player_leader_effect.png", "res://Assets/Images/Cards/cards_enemy_leader_effect.png"};

        if (isEnemy)
        {
            cardBackground.Color = Color.FromString("#5f0f5c", Colors.Purple);
        }

        if (isBoss)
        {
            effectTexture.Texture = new CompressedTexture2D
            {
                ResourcePath = effectsPathList[Convert.ToInt32(isEnemy)],
                LoadPath = effectsPathList[Convert.ToInt32(isEnemy)]
            };

            effectTexture.Visible = true;
        }


        //TODO: element & Icon
        // Texture2D icon, CardElements element, 
        // charcaterIcon.Texture = icon;

    }

    public void EditAllCardInformation(Card card)
    {
        
    }
}
