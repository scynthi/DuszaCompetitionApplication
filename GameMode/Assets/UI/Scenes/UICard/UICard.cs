using Godot;
using System;
using System.Buffers;

public partial class UICard : Control
{   
    [Signal] public delegate void CardClickedEventHandler(UICard card);

    [Export] private Label damageLabel;
    [Export] private Label healthLabel;  
    [Export] private Label nameLabel;  

    [Export] private TextureRect charcaterIcon;
    [Export] private TextureRect effectTexture;
    [Export] private TextureRect elementTexture;
    [Export] private TextureRect heartBuffedTexture;
    [Export] private TextureRect damageBuffedTexture;
    public Card OwnerCard { get; private set; }
    const byte DEFAULT_FONTSIZE = 19; 
    const byte DECREASED_FONTSIZE = 17; 

    public bool isEnemy;

    public UICard() {}

    public UICard(Card ownerCard)
    {
        OwnerCard = ownerCard;
    }

    public void SetOwnerCard(Card ownerCard)
    {
        OwnerCard = ownerCard;
    }

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

    private string _iconPath = Global.gameManager.uiPackedSceneReferences.ManTexture.ResourcePath;

    public string CardIcon
    {
        get { return _iconPath;}
        private set {_iconPath = value;}
    }

    public string CardName
    {
        private set {}
        get {return nameLabel.Text;}
    }
    
    public Card CreateCardInstance()
    {
        Card card = new(CardName, int.Parse(damageLabel.Text), int.Parse(healthLabel.Text), CardElement, CardIcon);
        card.HealthChanged = heartBuffedTexture.Visible;
        card.DamageChanged = damageBuffedTexture.Visible;
        return card;
    }

    public Vector2 UIPosition
    {
        get => Position; 
        set => Position = value; 
    }

    public void EditAllCardInformation(Card card)
    {
        UpdateIconForCardInstace(card);
        EditElement(card.CardElement);
        EditName(card.Name);
        EditHealth(card);
        EditDamage(card);
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

    public void EditAllCardInformation(string icon, CardElements element = CardElements.EARTH, string name = "Please Holder", int hp = 10, int damage = 2, bool isEnemy = false)
    {
        this.isEnemy = isEnemy;
        EditElement(element);
        EditName(name);
        EditHealth(hp);
        EditDamage(damage);

        if (icon != null) EditIcon(icon);
    }

    public void EditDamage(int damage)
    {
        if (damage == 0) damage = 1;
        damageLabel.Text =  Math.Clamp(damage, 1, 100).ToString();  
    }

    public void EditDamage(Card card)
    {
        int damage = card.BaseDamage;
        if (damage == 0) damage = 1;
        damageLabel.Text =  Math.Clamp(damage, 1, 100).ToString();
        if (card.DamageChanged) damageBuffedTexture.Visible = true;
    }

    public void EditHealth(int hp)
    {
        if (hp == 0) hp = 1;
        healthLabel.Text = Math.Clamp(hp, 1, 100).ToString();
    }    
    
    public void EditHealth(Card card)
    {
        int hp = card.Health;
        if (hp == 0) hp = 1;
        healthLabel.Text = Math.Clamp(hp, 1, 100).ToString();
        if (card.HealthChanged) heartBuffedTexture.Visible = true;
    }

    public void EditName(string name)
    {
        if (name.Replace(" ", "") == "") name = "Please Holder";

        nameLabel.Text = name;
        if (name.Length > 12)
        {
            nameLabel.AddThemeFontSizeOverride("font_size", DECREASED_FONTSIZE);
        } else
        {
            nameLabel.AddThemeFontSizeOverride("font_size", DEFAULT_FONTSIZE);
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
        if (card.Icon == null) card.Icon = CardIcon;
    }

    private ImageTexture CreateTexture(string resourcePath)
    {
        if (resourcePath == null || resourcePath == "") resourcePath = Global.gameManager.uiPackedSceneReferences.ManTexture.ResourcePath;

        CardIcon = resourcePath;
        FileAccess file = FileAccess.Open(resourcePath, FileAccess.ModeFlags.Read);
        byte[] buffer = file.GetBuffer((long)file.GetLength());
        Image image = new Image();
        Error err = image.LoadPngFromBuffer(buffer);

        if (err != Error.Ok) GD.PrintErr($"Failed to load image! {resourcePath}");

        return ImageTexture.CreateFromImage(image);
    }

    public void InteractButtonClicked()
    {
        EmitSignal(SignalName.CardClicked, this);
    }
}
