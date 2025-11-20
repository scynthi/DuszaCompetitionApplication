using Godot;
using System;

public partial class UICard : Control
{   
    Label damageLabel;
    Label healthLabel;  
    Label nameLabel;  

    ColorRect cardBackground;
    TextureRect charcaterIcon;
    TextureRect effectTexture;
    TextureRect elementTexture;

    public bool isBoss;
    public bool isEnemy;

    public override void _Ready()
    {
        damageLabel = GetNode<Label>("Axe/Damage");
        healthLabel = GetNode<Label>("Heart/Health");  
        nameLabel = GetNode<Label>("Control/Name");  

        cardBackground = GetNode<ColorRect>("Control/Background");
        charcaterIcon = GetNode<TextureRect>("Character");
        effectTexture = GetNode<TextureRect>("Effect");
        elementTexture = GetNode<TextureRect>("Element");
    }

    public void EditAllCardInformation(Card card)
    {
        EditElement(card.CardElement);
        EditName(card.Name);
        EditHealth(card.Health);
        EditDamage(card.Damage);
    }

    public void EditAllCardInformation(string icon = "res://Assets/Images/Entities/Heroes/man.png", CardElements element = CardElements.EARTH, string name = "Please Holder", int hp = 10, int damage = 2, bool isEnemy = false, bool isBoss = false)
    {
        this.isBoss = isBoss;
        this.isEnemy = isEnemy;
        EditEffect();
        EditElement(element);
        EditName(name);
        EditHealth(hp);
        EditDamage(damage);

        if (isEnemy)
        {
            cardBackground.Color = Color.FromString("#5f0f5c", Colors.Purple);
        }

        if (icon != null)
        {
            EditIcon(icon);
        }
    }

    public void EditEffect()
    {
        string[] effectsPathList = {"res://Assets/Images/Cards/cards_player_leader_effect.png", "res://Assets/Images/Cards/cards_enemy_leader_effect.png"};

        if (isBoss)
        {
            effectTexture.Texture = CreateTexture(effectsPathList[Convert.ToInt32(isEnemy)]);
        } else
        {
            effectTexture.Texture = CreateTexture("res://Assets/Images/Cards/cards_border.png");
        }
    }

    public void EditDamage(int damage)
    {
        if (damage == 0) damage = 1;
        healthLabel.Text = damage.ToString();  
    }

    public void EditHealth(int hp)
    {
        if (hp == 0) hp = 1;
        healthLabel.Text = hp.ToString();  
    }

    public void EditName(string name)
    {
        if (name.Replace(" ", "") == "") name = "Please Holder";

        nameLabel.Text = name;
        if (name.Length > 13)
        {
            nameLabel.AddThemeFontSizeOverride("font_size", 10);
        } else
        {
            nameLabel.AddThemeFontSizeOverride("font_size", 16);
        }
    }

    public void EditElement(CardElements element)
    {
        string[] elementPathList = {"res://Assets/Images/Elements/fold.png", "res://Assets/Images/Elements/levego.png", "res://Assets/Images/Elements/viz.png", "res://Assets/Images/Elements/tuz.png"};
        elementTexture.Texture = CreateTexture(elementPathList[(int)element]);
    }

    public void EditIcon(string icon)
    {
        charcaterIcon.Texture = CreateTexture(icon);
    }

    private ImageTexture CreateTexture(string resourcePath)
    {
        return ImageTexture.CreateFromImage(Image.LoadFromFile(resourcePath));
    }
}
