using Godot;
using System;

public partial class UIBossCard : Control
{   
    [Signal] public delegate void CardClickedEventHandler(UIBossCard bossCard);

    [Export] private Label damageLabel;
    [Export] private Label healthLabel;  
    [Export] private Label nameLabel;  

    [Export] private TextureRect charcaterIcon;
    [Export] private TextureRect effectTexture;
    [Export] private TextureRect elementTexture;
    [Export] private TextureRect heartBuffedTexture;
    [Export] private TextureRect damageBuffedTexture;
    public BossCard OwnerCard { get; private set; }

    const byte DEFAULT_FONTSIZE = 17; 
    const byte DECREASED_FONTSIZE = 16; 

    public bool isEnemy;

    public UIBossCard() {}

    public UIBossCard(BossCard ownerCard)
    {
        OwnerCard = ownerCard;
    }
    public void SetOwnerCard(BossCard ownerCard)
    {
        OwnerCard = ownerCard;
    }
    private BossCard _bossCard;
    public BossCard BossCard
    {
        private set {_bossCard = value;}
        get {return _bossCard;}
    }
    private CardElements _cardElement = CardElements.FIRE;
    public CardElements CardElement {
        private set {_cardElement = value;} 
        get {return _cardElement;}
    }


    public int CardDamage {
        private set {} 
        get {return Convert.ToInt32(damageLabel.Text);}
    }

    public int CardHealth {
        private set {} 
        get {return Convert.ToInt32(healthLabel.Text);}
    }

    public string CardName
    {
        private set {}
        get {return nameLabel.Text;}
    }

    private string _iconPath = Global.gameManager.uiPackedSceneReferences.ManTexture.ResourcePath;

    public string CardIcon
    {
        get { return _iconPath;}
        private set {_iconPath = value;}
    }
    
    public BossCard CreateBossCardInstance()
    {
        return BossCard;
    }

    public Vector2 UIPosition
    {
        get => Position; 
        set => Position = value; 
    }

    public void EditAllCardInformation(Card baseCard, BossDouble evolveType, string addedName = "lord ", bool isEnemy = false)
    {
        this.isEnemy = isEnemy;
        BossCard = new BossCard(baseCard, addedName, evolveType);
        UpdateIconForCardInstace(BossCard);
        EditName(BossCard.Name);
        EditIcon(BossCard.Icon);
        EditDamage(BossCard);
        EditHealth(BossCard);
        EditElement(BossCard.CardElement);
        SetupEffect();
    }

    public void EditAllCardInformation(BossCard bossCard, bool isEnemy = false)
    {
        this.isEnemy = isEnemy;
        BossCard = bossCard;
        UpdateIconForCardInstace(BossCard);
        EditIcon(BossCard.Icon);
        EditName(BossCard.Name);
        EditDamage(BossCard);
        EditHealth(BossCard);
        EditElement(BossCard.CardElement);
        SetupEffect();
    }

    public void EditAllCardInformation(UIBossCard UIBossCard)
    {
        EditName(UIBossCard.CardName);
        EditDamage(UIBossCard.CardDamage);
        EditHealth(UIBossCard.CardHealth);
        EditElement(UIBossCard.CardElement);
        EditIcon(UIBossCard.CardIcon);
        SetupEffect();
    }

    private void SetupEffect()
    {
        if (isEnemy) effectTexture.Texture = CreateTexture("res://Assets/Images/Cards/cards_enemy_leader_effect.png");
    }

    public void EditDamage(int damage)
    {
        if (damage == 0) damage = 1;
        damageLabel.Text =  Math.Clamp(damage, 1, 100).ToString();  
    }

    public void EditDamage(BossCard card)
    {
        int damage = card.Damage;
        if (damage == 0) damage = 1;
        damageLabel.Text =  Math.Clamp(damage, 1, 100).ToString();
        if (card.DamageChanged) damageBuffedTexture.Visible = true;
    }

    public void EditHealth(int hp)
    {
        if (hp == 0) hp = 1;
        healthLabel.Text = Math.Clamp(hp, 1, 100).ToString();
    }    
    
    public void EditHealth(BossCard card)
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
            nameLabel.AddThemeFontSizeOverride("font_size", DEFAULT_FONTSIZE);
        } else
        {
            nameLabel.AddThemeFontSizeOverride("font_size", DECREASED_FONTSIZE);
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

    public void UpdateIconForCardInstace(BossCard card)
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
