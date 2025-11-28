using Godot;
using System;
using System.Collections.Generic;

public partial class CardShopArea : Control
{
    [Export] AnimationPlayer animationPlayer;
    [Export] AnimationPlayer animationWorldPlayer;

    [Export] Control cardHolder;
    [Export] Button reRollButton;
    [Export] Button buyButton;
    [Export] Label moneyLabel;
    [Export] Label buyPriceLabel;

    private Tween _shakeTween;
    private Card currCard;
    private int _currPrice;

    public override void _Ready()
    {
        _SetNewCard();

        reRollButton.Pressed += _ReRolled;
        buyButton.Pressed += _CardBought;
    }

    public Card GetNewCard()
    {
        List<Card> randomSelection = [];

        foreach (Card card in Global.gameManager.saverLoader.currSaveFile.WorldCards)
        {
            if (!Utility.WorldObjectListToNameList(Global.gameManager.saverLoader.currSaveFile.player.Collection).Contains(card.Name))
            {
                GD.Print(card.Name);
                randomSelection.Add(card);
            }
        }

        if (randomSelection.Count > 0)
        {
            return randomSelection[GD.RandRange(0,randomSelection.Count - 1)];
        }

        foreach (Control node in cardHolder.GetChildren())
        {
            node.QueueFree();
        }
        currCard = null;

        reRollButton.Disabled = true;
        buyButton.Disabled = true;

        return null;
    }

    public async void _ReRolled()
    {
        if (animationPlayer.IsPlaying()) return;
        ShakeControl();
        if (Global.gameManager.saverLoader.currSaveFile.player.Money >= 10.0f)
        {
            Global.gameManager.saverLoader.currSaveFile.player.Money -= 10;
            animationPlayer.Play("Recard");
            animationWorldPlayer.Play("Glare");
            
        }
        animationWorldPlayer.Play("Flare");
    }

    public async void _CardBought()
    {
        if (animationPlayer.IsPlaying()) return;
        
        ShakeControl();
        if (Global.gameManager.saverLoader.currSaveFile.player.Money >= _currPrice)
        {
            Global.gameManager.saverLoader.currSaveFile.player.Money -= _currPrice;
            Global.gameManager.saverLoader.currSaveFile.player.TryAddCardToCollection(currCard);
            animationPlayer.Play("Recard");
            animationWorldPlayer.Play("Glare");

        }
        animationWorldPlayer.Play("Flare");

        
    }

    public void _SetNewCard()
    {
        currCard = GetNewCard();
        if (currCard == null) return;
        Utility.AddUiCardUnderContainer(currCard, cardHolder);
        
        _currPrice = currCard is BossCard ? 10 : 15;
        buyPriceLabel.Text = "$"+Convert.ToString(_currPrice);
        
        UpdateMoneyText();
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
			_shakeTween.TweenProperty(cardHolder, "rotation_degrees", targetRotation, stepDuration);
		}
		
		_shakeTween.TweenProperty(cardHolder, "rotation_degrees", 0.0f, stepDuration);
	}

    public void UpdateMoneyText()
    {
        moneyLabel.Text = Convert.ToString(Global.gameManager.saverLoader.currSaveFile.player.Money);
    }
}
