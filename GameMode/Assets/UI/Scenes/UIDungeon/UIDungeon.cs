using Godot;
using System;

public partial class UIDungeon : Control
{
    RichTextLabel nameLabel;
    TextureRect iconTexture;
    Button enterButton;


    public string DungeonName 
    {
        private set {} 
        get {return nameLabel.Text;}
    }
    public DungeonTypes DungeonType {private set; get;}


    private bool _previewMode = true;

    [Export] public bool PreviewMode
    {
        set
        {
            _previewMode = value;
            if (enterButton != null) enterButton.Visible = !value;
        } 
        get {return _previewMode;}
    }

    public override void _Ready()
    {
        nameLabel = GetNode<RichTextLabel>("Label");
        iconTexture = GetNode<TextureRect>("TextureRect");
        enterButton = GetNode<Button>("Button");
        PreviewMode = false;
    }

    public void SetUpDungeon(UIDungeon dungeon)
    {
        EditName(dungeon.DungeonName);
        EditType(dungeon.DungeonType);
    }

    public void SetUpDungeon(Dungeon dungeon)
    {
        EditName(dungeon.Name);
        EditType(dungeon.DungeonType);
    }

    public void SetUpDungeon(string name = "Teszt dungeon", DungeonTypes type = DungeonTypes.simple)
    {
        EditName(name);
        EditType(type);
    }


    public void EditName(string name)
    {
        if (name.Replace(" ", "") == "") return;
        nameLabel.Text = name;
    }

    public void EditType(DungeonTypes type)
    {
        DungeonType = type;
        string[] iconPathList = {"res://Assets/Images/Portal/portal_frame_egyszeru.png", "res://Assets/Images/Portal/portal_frame_kis.png", "res://Assets/Images/Portal/portal_frame_nagy.png"};
        // iconTexture.Texture = CreateTexture(iconPathList[(int)type]);
        iconTexture.Texture = GD.Load<Texture2D>(iconPathList[(int)type]);
    }

    private ImageTexture CreateTexture(string resourcePath)
    {
        return ImageTexture.CreateFromImage(Image.LoadFromFile(resourcePath));
    }

    public Dungeon CreateInstance()
    {
        return new Dungeon(DungeonName, DungeonType, (DungeonRewardTypes)DungeonType);
    }
}
