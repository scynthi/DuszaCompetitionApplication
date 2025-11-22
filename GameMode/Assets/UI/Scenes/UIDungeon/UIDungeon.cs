using Godot;
using System;

public partial class UIDungeon : Control
{
    Label nameLabel;
    TextureRect iconTexture;
    Button enterButton;


    public string dungeonName 
    {
        private set {} 
        get {return nameLabel.Text;}
    }
    public DungeonTypes dungeonType {private set; get;}


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
        nameLabel = GetNode<Label>("HBox/VBox/Label");
        iconTexture = GetNode<TextureRect>("HBox/VBox/TextureRect");
        enterButton = GetNode<Button>("HBox/VBox/Button");
        PreviewMode = true;
    }

    public void SetUpdungeon(Dungeon dungeon)
    {
        EditName(dungeon.Name);
        EditType(dungeon.DungeonType);
    }

    public void SetUpdungeon(string name = "Teszt dungeon", DungeonTypes type = DungeonTypes.simple)
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
        dungeonType = type;
        string[] iconPathList = {"res://Assets/Images/Portal/portal_frame_egyszeru.png", "res://Assets/Images/Portal/portal_frame_kis.png", "res://Assets/Images/Portal/portal_frame_nagy.png"};
        iconTexture.Texture = CreateTexture(iconPathList[(int)type]);
    }

    private ImageTexture CreateTexture(string resourcePath)
    {
        return ImageTexture.CreateFromImage(Image.LoadFromFile(resourcePath));
    }



}
