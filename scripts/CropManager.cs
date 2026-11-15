using Godot;
using System;
using System.Linq;

namespace HelpingHaven;

public partial class CropManager : Node2D
{
    public enum CropState {
        JustPlanted,
        StageTwo
    }

    private ushort CarrotCount { get; set; }
    private ushort TomatoCount { get; set; }

    [Export]
    private TileMapLayer TillLayer { get; set; }
    [Export]
    private TileMapLayer CropLayer { get; set; }

    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public void PrimaryActionPressed(Vector2 position, Direction facing)
    {
        GD.Print("Primary Pressed");

        var localizedPosition = ToLocal(position);

        var current_tile = CropLayer.LocalToMap(localizedPosition);

        var target_tile = GetTargetPosition(current_tile, facing);

        GD.Print("Targeting Tile {}", target_tile);

        CropLayer.EraseCell(target_tile);

        // (1-4), 12
        
    }

    private Vector2I GetTargetPosition(Vector2I currentPosition, Direction facing)
    {
        Vector2I target_tile_pos;
        switch (facing)
        {
            case Direction.Up:
                target_tile_pos = currentPosition += Vector2I.Up;
                break;
            case Direction.Down:
                target_tile_pos = currentPosition += Vector2I.Down;
                break;
            case Direction.Left:
                target_tile_pos = currentPosition += Vector2I.Left;
                break;
            case Direction.Right:
                target_tile_pos = currentPosition += Vector2I.Left;
                break;
            default:
                // Impossible Case
                target_tile_pos = currentPosition;
                break;
        }

        return target_tile_pos;
    }

    
}
