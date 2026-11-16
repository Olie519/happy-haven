using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace HelpingHaven;

public enum ToolSelection {
    None,
    WateringCan,
    CarrotSeed,
    TomatoSeed
}

public partial class CharacterController : Node2D
{
    [Export]
    private TileMapLayer UILayer { get; set; }

    [Export]
    private Camera2D Camera { get; set; }

    private ToolSelection SelectedTool { get; set; }
    private CharacterBody2D Body { get; set; }
    private Direction Facing { get; set; }

    [Signal]
    public delegate void PrimaryActionEventHandler(Vector2 Position, Direction facing, int tool);

    public override void _EnterTree()
    {
        Body = GetChild<CharacterBody2D>(0);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        Camera.GlobalPosition = Body.GlobalPosition;

        if(Input.IsActionJustPressed("primary")) {
            EmitSignal(SignalName.PrimaryAction, Body.GlobalPosition, (int)Facing, (int)SelectedTool);
        }

        var tool = (int)SelectedTool;

        if(Input.IsActionJustPressed("last_tool"))
        {
            if( (tool - 1) >= 0)
                tool -= 1;

            UpdateToolIcon((ToolSelection)tool);
            
        } else if(Input.IsActionJustPressed("next_tool"))
        {
            if( (tool + 1) <= 3)
                tool += 1;

            UpdateToolIcon((ToolSelection)tool);
        }


        SelectedTool = (ToolSelection)tool;
        
    }

    private void UpdateToolIcon(ToolSelection tool)
    {
        switch (tool) {
            case ToolSelection.None:
                UILayer.SetCell(new Vector2I(-7,-4), 0, new Vector2I(9,7));
                break;
            case ToolSelection.WateringCan:
                UILayer.SetCell(new Vector2I(-7,-4), 0, new Vector2I(6,7));
                break;
            case ToolSelection.CarrotSeed:
                UILayer.SetCell(new Vector2I(-7,-4), 0, new Vector2I(15,0));
                break;
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
