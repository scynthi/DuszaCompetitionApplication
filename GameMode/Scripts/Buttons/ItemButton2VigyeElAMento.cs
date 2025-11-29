using Godot;
using System;

public partial class ItemButton2VigyeElAMento : Control
{
	[Signal]
    public delegate void Send_ItemEventHandler(int item);

	[Export] public ItemType itemType;
    [Export] public DescriptionBox descriptionBox;
    [Export] public TextureRect iconHolder;
	[Export] public Button useButton;
	private IItem item;

    private Tween _tween;
    private Tween _useTween;

    private Vector2 _originalPosition;
    private Vector2 _useOriginalPosition;

    private float _hoverOffset = -10f;
    private float _useHoverOffset = 52f;

    private bool _isHovering = false;

    public override void _Ready()
    {
        _originalPosition = Position;
        _useOriginalPosition = useButton.Position;

        MouseEntered += Hover;
        
		item = Items.CreateItemFromType(itemType);
        iconHolder.Texture = Utility.LoadTextureFromPath(item.Icon);
    }

    public override void _Process(double delta)
    {
        if(!_isHovering)return;
        
        if (!GetGlobalRect().HasPoint(GetGlobalMousePosition()))
        {
            _isHovering = false;
            NoHover();
        }
    }

	public void _UsePressed()
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());

        EmitSignal(SignalName.Send_Item, (int)itemType);
        descriptionBox.UpdateDescription(item.Name, item.Description, Global.gameManager.saverLoader.currSaveFile.player.ReturnItemAmount(item));
    }

	private void Hover()
    {   
        if (_isHovering) return;
        _isHovering = true;
        descriptionBox.ShowDescription(item.Name, item.Description, Global.gameManager.saverLoader.currSaveFile.player.ReturnItemAmount(item), GlobalPosition + new Vector2(0, -5), Size.X);
    }
	private void NoHover()
    {
        descriptionBox.HideDescription();
    }

    public void Disable()
    {
        useButton.Disabled = true;
    }
	
}
