using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node
{
    [Export] private Node World;

    [Export] private AnimationPlayer transitionAnimator;

    public Node currentWorldScene { private set; get; }

    public static class ScenePaths
    {
        public const string MainMenu = "uid://cgalnmmbimcuw";
    }

    public override void _Ready()
    {
        Global.gameManager = this;

        if (World.GetChildCount() > 0)
        {
            currentWorldScene = World.GetChild(0);
        }
    }

    public async Task ChangeWorldScene(StringName newScenePath)
    {
        transitionAnimator.Play("SceneTransition");

        await ToSignal(transitionAnimator, AnimationPlayer.SignalName.AnimationFinished);

        if (currentWorldScene != null)
        {
            currentWorldScene.QueueFree();   
        }

        var newScene = GD.Load<PackedScene>(newScenePath).Instantiate();
        World.AddChild(newScene);
        currentWorldScene = newScene;

        GD.Print("Should be loaded");

        transitionAnimator.Play("SceneTransitionOut");
    }

}
