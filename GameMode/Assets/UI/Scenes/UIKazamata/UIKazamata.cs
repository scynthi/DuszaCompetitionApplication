using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class UIKazamata : Control
{

    // TODO: KAZAMATA code reward option

    Label nameLabel;
    TextureRect iconTexture;
    Button enterButton;


    public string kazamataName 
    {
        private set {} 
        get {return nameLabel.Text;}
    }
    public DungeonTypes kazamataType {private set; get;}


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

    public void SetUpKazamata(Dungeon kazamata)
    {
        EditName(kazamata.Name);
        EditType(kazamata.DungeonType);
    }

    public void SetUpKazamata(string name = "Teszt Kazamata", DungeonTypes type = DungeonTypes.simple)
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
        kazamataType = type;
        string[] iconPathList = {"res://Assets/Images/Portal/portal_frame_egyszeru.png", "res://Assets/Images/Portal/portal_frame_kis.png", "res://Assets/Images/Portal/portal_frame_nagy.png"};
        iconTexture.Texture = CreateTexture(iconPathList[(int)type]);
    }

    private ImageTexture CreateTexture(string resourcePath)
    {
        return ImageTexture.CreateFromImage(Image.LoadFromFile(resourcePath));
    }



}
