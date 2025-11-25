using Godot;
using System;

public partial class UICard : Control
{   
    [Signal] public delegate void CardClickedEventHandler(UICard card);

    [Export] private Label damageLabel;
    [Export] private Label healthLabel;  
    [Export] private Label nameLabel;  

    [Export] private ColorRect cardBackground;
    [Export] private TextureRect charcaterIcon;
    [Export] private TextureRect effectTexture;
    [Export] private TextureRect elementTexture;

    public bool isEnemy;

    public int CardDamage {
        private set{} 
        get {return Convert.ToInt32(damageLabel.Text);}
    }

    public int CardHealth {
        private set{} 
        get {return Convert.ToInt32(healthLabel.Text);}
    }

    private CardElements _cardElement = CardElements.FIRE;

    public CardElements CardElement {
        private set {_cardElement = value;} 
        get {return _cardElement;}
    }

    public Texture2D CardIcon
    {
        private set {}
        get {return charcaterIcon.Texture;}
    }

    public string CardName
    {
        private set {}
        get {return nameLabel.Text;}
    }
    
    // TODO: update later with mate
    public Card CreateCardInstance()
    {
        return new Card(CardName, int.Parse(damageLabel.Text), int.Parse(healthLabel.Text), CardElement, CardIcon);
    }

    public void EditAllCardInformation(Card card)
    {
        UpdateIconForCardInstace(card);
        EditElement(card.CardElement);
        EditName(card.Name);
        EditHealth(card.Health);
        EditDamage(card.Damage);
        EditIcon(card.Icon);
    }

    public void EditAllCardInformation(UICard card)
    {
        isEnemy = card.isEnemy;
        EditElement(card.CardElement);
        EditName(card.CardName);
        EditHealth(card.CardHealth);
        EditDamage(card.CardDamage);
        EditIcon(card.CardIcon);
    }

    public void EditAllCardInformation(string icon = "res://Assets/Images/Entities/Heroes/man.png", CardElements element = CardElements.EARTH, string name = "Please Holder", int hp = 10, int damage = 2, bool isEnemy = false)
    {
        this.isEnemy = isEnemy;
        EditElement(element);
        EditName(name);
        EditHealth(hp);
        EditDamage(damage);

        if (isEnemy) cardBackground.Color = Color.FromString("#5f0f5c", Colors.Purple);
        if (icon != null) EditIcon(icon);
    }

    public void EditDamage(int damage)
    {
        if (damage == 0) damage = 1;
        damageLabel.Text =  Math.Clamp(damage, 1, 100).ToString();  
    }

    public void EditHealth(int hp)
    {
        if (hp == 0) hp = 1;
        healthLabel.Text = Math.Clamp(hp, 1, 100).ToString();  
    }

    public void EditName(string name)
    {
        if (name.Replace(" ", "") == "") name = "Please Holder";

        nameLabel.Text = name;
        if (name.Length > 12)
        {
            nameLabel.AddThemeFontSizeOverride("font_size", 8);
        } else
        {
            nameLabel.AddThemeFontSizeOverride("font_size", 10);
        }
    }

    public void EditElement(CardElements element)
    {
        CardElement = element;
        Resource[] elementPathList = {GD.Load("uid://flp5hfrmldcm"), GD.Load("uid://ubulfv30qw2x"), GD.Load("uid://dfmsxlsr24dcu"), GD.Load("uid://doww5jvob8iw2")};
        
        elementTexture.Texture = (Texture2D)elementPathList[(int)element];
    }

    public void EditIcon(string icon)
    {
        charcaterIcon.Texture = CreateTexture(icon);
    }

    public void EditIcon(Image icon)
    {
        charcaterIcon.Texture = ImageTexture.CreateFromImage(icon);
    }

    public void EditIcon(Texture2D icon)
    {
        charcaterIcon.Texture = icon;
    }

    public void UpdateIconForCardInstace(Card card)
    {
        if (card.Icon == null) card.Icon = CardIcon.ResourcePath;
    }

    private ImageTexture CreateTexture(string resourcePath)
    {
        return ImageTexture.CreateFromImage(Image.LoadFromFile(resourcePath));
    }

    public void InteractButtonClicked()
    {
        EmitSignal(SignalName.CardClicked, this);
    }
}
