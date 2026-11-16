using Godot;
using GodotPlugins.Game;
using System;

public partial class IntroController : Node2D
{
    [Export]
    public PackedScene MainScene { get; set; }

    [Export]
    public Sprite2D SecondCard { get; set; }

    [Export]
    public Camera2D Camera { get; set; }

    public override void _Process(double delta)
    {
        base._Process(delta);
            
        if(Input.IsActionJustPressed("primary"))
        {
            if (SecondCard.Visible){
                this.Visible = false;
                Camera.QueueFree();
                var scene = MainScene.Instantiate();
                AddSibling(scene);
            }
            SecondCard.Visible = true;
        }
    }
}
