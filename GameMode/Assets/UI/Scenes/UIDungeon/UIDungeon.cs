using Godot;
using System;
using System.Collections.Generic;

public partial class UIDungeon : Control
{
    [Export] Label nameRichText;
    [Export] Label nameLabel;
    [Export] TextureRect iconTexture;
    [Export] Button enterButton;
    [Export] Control ClickCatcher;

    [Signal] public delegate void DungeonClickedEventHandler(UIDungeon uidungeon);

    private string _dungeonName = "Teszt Kazamata";

    public string DungeonName 
    {
        private set {} 
        get {return _dungeonName;}
    }
    public DungeonTypes DungeonType {private set; get;}
    public DungeonRewardTypes DungeonReward {private set; get;}

    private bool _previewMode = true;

    [Export] public bool PreviewMode
    {
        set
        {
            _previewMode = value;
            nameLabel.Visible = value;
            nameRichText.Visible = !value;
            enterButton.Visible = !value;
            iconTexture.CustomMinimumSize = value ? new Vector2(220,220) : new Vector2(30,120);
            ClickCatcher.MouseFilter = value ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;
        } 
        get {return _previewMode;}
    }

    public override void _Ready()
    {
        PreviewMode = true;
    }

    public void SetUpDungeon(UIDungeon dungeon)
    {
        EditName(dungeon.DungeonName);
        EditType(dungeon.DungeonType);
        EditReward(dungeon.DungeonReward);
    }

    public void SetUpDungeon(Dungeon dungeon)
    {
        EditName(dungeon.Name);
        EditType(dungeon.DungeonType);
        EditReward(dungeon.DungeonReward);
    }

    public void SetUpDungeon(string name = "Teszt kazamata", DungeonTypes type = DungeonTypes.simple, DungeonRewardTypes rewardType = DungeonRewardTypes.health)
    {
        EditName(name);
        EditType(type);
        EditReward(rewardType);
    }


    public void EditName(string name)
    {
        if (name.Replace(" ", "") == "") return;

        nameLabel.Text = name;
        nameRichText.Text = name;
        _dungeonName = name;
    }

    public void EditReward(DungeonRewardTypes rewardType)
    {
        DungeonReward = rewardType; 
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

    public Dungeon CreateDungeonInstance()
    {
        List<Card> DungeonCards = new List<Card>();
        return new Dungeon(DungeonName, DungeonType, DungeonReward, DungeonCards);
    }

    public void InteractButtonClicked()
    {
        EmitSignal(SignalName.DungeonClicked, this);
    }
}
