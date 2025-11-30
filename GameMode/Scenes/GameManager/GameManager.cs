using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node
{
    [Export] private Node World;

    [Export] private AnimationPlayer transitionAnimator;
    [Export] public SaverLoader saverLoader;
    [Export] public UiAudioController audioController;
    [Export] public UiPackedSceneReferences uiPackedSceneReferences;
    [Export] public ColorRect ctrShaderRect;

    public Node currentWorldScene { private set; get; }

    public static class ScenePaths
    {
        public const string MainMenu = "res://Scenes/UI/MainMenu/MainMenu.tscn";
        public const string EditorMenu = "res://Scenes/UI/GameMaster/GameMaster.tscn";
        public const string DungeonMap = "res://Scenes/DungeonMap/DungeonMap.tscn";
        public const string FightMap = "res://Scenes/FightScene/FightScene.tscn";
        public const string ShopMenu = "res://Scenes/ShopScene/ShopMenu.tscn";
    }

    public override void _Ready()
    {
        Global.gameManager = this;

        if (World.GetChildCount() > 0)
        {
            currentWorldScene = World.GetChild(0);
        }

        Global.gameManager.audioController.PlayMusicAndEnvSounds(Global.gameManager.audioController.audioBank.MainMenuMusic, Global.gameManager.audioController.audioBank.MainMenuAmbiance);
    }

    public async Task ChangeWorldScene(StringName newScenePath)
    {
        audioController.PlaySFX(Global.gameManager.audioController.audioBank.transitionSounds[0]);


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
        audioController.PlaySFX(Global.gameManager.audioController.audioBank.transitionSounds[1]);
    }

}
