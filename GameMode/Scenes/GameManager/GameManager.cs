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
        public const string MainMenu = "res://Scenes/UI/MainMenu/MainMenuWorld.tscn";
        public const string EditorMenu = "res://Scenes/UI/GameMaster/GameMasterWorld.tscn";
        public const string DungeonMap = "res://Scenes/DungeonMap/DungeonMap.tscn";
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
