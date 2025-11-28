using Godot;
using System;

public partial class DamageLabel : Label
{
    public override void _Ready()
    {
        // Center the pivot for scaling
        PivotOffset = Size / 2;

        // Create a tween
        var damageLabelTween = GetTree().CreateTween();
        damageLabelTween.SetParallel(true);

        // Scale down slightly over 0.2s
        damageLabelTween.TweenProperty(this, "scale", new Vector2(0.7f, 0.7f), 0.2f);

        // Scale to zero after 0.4s delay over 0.5s
        damageLabelTween.TweenProperty(this, "scale", Vector2.Zero, 0.5f).SetDelay(0.4f);

        // Move up over 2s after 0.4s delay
        damageLabelTween.TweenProperty(this, "position", Position + new Vector2(0, -280), 2f).SetDelay(0.4f);

        // Queue free after tween finishes
        damageLabelTween.Finished += () => QueueFree();
    }
}
