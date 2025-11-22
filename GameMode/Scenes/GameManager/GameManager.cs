using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node
{
    [Export] private Node World;

    [Export] private AnimationPlayer transitionAnimator;
    [Export] public SaveLoadSystem saverLoader;
    [Export] public UiAudioController audioController;

    public Node currentWorldScene { private set; get; }

    public static class ScenePaths
    {
        public const string MainMenu = "uid://cgalnmmbimcuw";
        public const string EditorMenu = "uid://t05qnqewfh32";
        public const string DungeonMap = "uid://oihoapowoy2t";
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

        transitionAnimator.Play("SceneTransitionOut");
    }

}
