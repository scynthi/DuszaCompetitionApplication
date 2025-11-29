using Godot;
using System;

public partial class ItemButton : Control
{
	[Signal]
    public delegate void Send_ItemEventHandler(int item);
	[Signal]
    public delegate void Send_Item_RemovedEventHandler(int item);
    [Signal]
    public delegate void Send_Item_AddedEventHandler(int item);

	[Export] public ItemType itemType;
    [Export] public DescriptionBox descriptionBox;
    [Export] public Label priceLabel;
    [Export] public TextureRect iconHolder;
	[Export] public Button buyButton;
    private IItem item;

    private Vector2 _originalPosition;
    private Vector2 _buyOriginalPosition;

    private Tween _tween;
    private Tween _buyTween;
    private Tween _shakeTween;

    private float _hoverOffset = -10f;
    private float _buyHoverOffset = 52f;

    private bool _isHovering = false;


    public override void _Ready()
    {
        _originalPosition = Position;
        _buyOriginalPosition = buyButton.Position;


        MouseEntered += Hover;

        buyButton.Pressed += _Pressed;

		item = Items.CreateItemFromType(itemType);
        iconHolder.Texture = Utility.LoadTextureFromPath(item.Icon);
        priceLabel.Text = "$" + Convert.ToString(item.Price);
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


	public void _Pressed()
    {
        Global.gameManager.audioController.PlaySFX(Global.gameManager.audioController.audioBank.clickSounds.PickRandom());

        EmitSignal(SignalName.Send_Item, (int)itemType);
        descriptionBox.UpdateDescription(item.Name, item.Description, Global.gameManager.saverLoader.currSaveFile.player.ReturnItemAmount(item));
        ShakeControl();
    }

    // public override void _Toggled(bool toggledOn)
    // {
    //     base._Toggled(toggledOn);
    //     if (toggledOn)
    //     {
    //         EmitSignal(SignalName.Send_Item_Added, (int)itemType);
    //         return;
    //     }
    //     EmitSignal(SignalName.Send_Item_Removed, (int)itemType);
    // }


	private void Hover()
    {   
        if (_isHovering) return;
        if (_tween != null && _tween.IsRunning())
        {
            _tween.Kill();
        }
        if (_buyTween != null && _buyTween.IsRunning())
        {
            _buyTween.Kill();
        }

        _isHovering = true;
        
        _tween = CreateTween();
        _tween.SetEase(Tween.EaseType.Out);
        _tween.SetTrans(Tween.TransitionType.Back);
        _tween.TweenProperty(this, "position",new Vector2(Position.X, Position.Y + _hoverOffset), 0.2);

        
        _buyTween = CreateTween();
        _buyTween.SetEase(Tween.EaseType.Out);
        _buyTween.SetTrans(Tween.TransitionType.Back);
        _buyTween.TweenProperty(buyButton, "position", new Vector2(buyButton.Position.X, _buyOriginalPosition.Y + _buyHoverOffset), 0.2);


        descriptionBox.ShowDescription(item.Name, item.Description, Global.gameManager.saverLoader.currSaveFile.player.ReturnItemAmount(item), GlobalPosition + new Vector2(0, -5), Size.X);

    }
	private void NoHover()
    {
        if (_tween != null && _tween.IsRunning())
        {
            _tween.Kill();
        }
        if (_buyTween != null && _buyTween.IsRunning())
        {
            _buyTween.Kill();
        }

        _tween = CreateTween();
        _tween.SetEase(Tween.EaseType.Out);
        _tween.SetTrans(Tween.TransitionType.Cubic);
        _tween.TweenProperty(this, "position", _originalPosition, 0.2);

        _buyTween = CreateTween();
        _buyTween.SetEase(Tween.EaseType.Out);
        _buyTween.SetTrans(Tween.TransitionType.Back);
        _buyTween.TweenProperty(buyButton, "position",new Vector2(buyButton.Position.X, _buyOriginalPosition.Y), 0.2);

        descriptionBox.HideDescription();
    }

    
    public void ShakeControl(float duration = 0.5f, float intensity = 10.0f)
	{
        if (_shakeTween != null && _shakeTween.IsRunning())
        {
            _shakeTween.Kill();
        }

        _shakeTween = CreateTween();
		
		_shakeTween.SetTrans(Tween.TransitionType.Sine);
		_shakeTween.SetEase(Tween.EaseType.InOut);
		
		int shakeSteps = 8;
		float stepDuration = duration / shakeSteps;
		
		for (int i = 0; i < shakeSteps; i++)
		{
			float shakeAmount = intensity * (1.0f - (float)i / shakeSteps);
			float targetRotation = 0.0f + (i % 2 == 0 ? shakeAmount : -shakeAmount);
			_shakeTween.TweenProperty(iconHolder, "rotation_degrees", targetRotation, stepDuration);
		}
		
		_shakeTween.TweenProperty(iconHolder, "rotation_degrees", 0.0f, stepDuration);
	}
}
