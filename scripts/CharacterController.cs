using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace HelpingHaven;

public partial class CharacterController : Node2D
{
    

    [Signal]
    public delegate void PrimaryActionEventHandler(Vector2 Position, Direction facing);

    private CharacterBody2D Body { get; set; }
    private Direction Facing { get; set; }

    public override void _EnterTree()
    {
        Body = GetChild<CharacterBody2D>(0);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if(Input.IsActionJustPressed("primary")) {
            EmitSignal(SignalName.PrimaryAction, Body.GlobalPosition, (int)Facing);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        var inputVector = GetInputVector();

        // TODO: Actually collide
        var _collidingBody = Body.MoveAndCollide(inputVector);
    }

    private Vector2 GetInputVector() 
    {
        var input = Vector2.Zero;

        if (Input.IsActionPressed("move_up")) {
            input += Vector2.Up;
            Facing = Direction.Up;
        }

        if (Input.IsActionPressed("move_down")) {
            input += Vector2.Down;
            Facing = Direction.Down;
        }

        if (Input.IsActionPressed("move_left")) {
            input += Vector2.Left;
            Facing = Direction.Left;
        }

        if (Input.IsActionPressed("move_right")) {
            input += Vector2.Right;
            Facing = Direction.Right;
        }

        return input;
    }
}
