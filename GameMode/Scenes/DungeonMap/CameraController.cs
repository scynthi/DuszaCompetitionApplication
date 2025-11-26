using Godot;
using System;

public partial class CameraController : Camera2D
{
    private bool ButtonDown = false;
    private Vector2 dragStartMousePos;
    private Vector2 dragStartCameraPos;

    int edgeMarign = 5;
    int cameraSpeed = 200;
    Vector2 mapSize = new Vector2(0, 720);
    Vector2 viewportSize = new Vector2(1280,720);


    public override void _Ready()
    {
        GD.Print(Global.gameManager.saverLoader.currSaveFile.WorldDungeons.Count);
        mapSize = new Vector2(GetViewportRect().Size.X * (int)Math.Ceiling((float)Global.gameManager.saverLoader.currSaveFile.WorldDungeons.Count / (float)6), mapSize.Y);
    }

    // I have no Idea what these do but it works (it was created from an unholy fusion dance of chatgpt and a random youtubers code)
    public override void _Process(double delta)
    {
        Position = new Vector2(Math.Clamp(Position.X, 0.0f, mapSize.X - viewportSize.X), 0.0f);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Middle)
        {
            if (mouseButton.Pressed)
            {
                ButtonDown = true;
                dragStartMousePos = mouseButton.Position;
                dragStartCameraPos = Position;
            }
            else
            {
                ButtonDown = false;
            }
        }
        
        if (@event is InputEventMouseMotion mouseMotion && ButtonDown)
        {
            Vector2 mouseDelta = dragStartMousePos - mouseMotion.Position;
            float newX = dragStartCameraPos.X + mouseDelta.X;
            float clampedX = Mathf.Clamp(newX, LimitLeft, LimitRight);
            
            if (clampedX != newX)
            {
                dragStartMousePos = mouseMotion.Position;
                dragStartCameraPos = new Vector2(clampedX,0);
            }
            
            Position = new Vector2(clampedX,0);
        }
    }

}
