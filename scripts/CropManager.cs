using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpingHaven;

public class CropDetails {
    public int Type { get; set;}
    public CropState State { get; set; }
    public bool Watered { get; set; }
    public Vector2I? Atlas { get; set; }
}

public enum CropState {
    OnlyTilled,
    JustPlanted,
    StageTwo,
    StageThree,
    Ready,
    Wilted
}

public partial class CropManager : Node2D
{
    public Dictionary<Vector2I, CropDetails> Crops = new();

    private ushort CarrotCount { get; set; }
    private ushort TomatoCount { get; set; }

    [Export]
    private TileMapLayer TillLayer { get; set; }
    [Export]
    private TileMapLayer CropLayer { get; set; }

    public void PrimaryActionPressed(Vector2 position, Direction facing, int tool)
    {
        GD.Print("Primary Pressed");

        var localizedPosition = ToLocal(position);

        var current_tile = CropLayer.LocalToMap(localizedPosition);

        var target_tile = GetTargetPosition(current_tile, facing);

        GD.Print("Targeting Tile: ", target_tile);

        switch ((ToolSelection)tool) {
            case ToolSelection.None:
                Pull(target_tile);
                Till(target_tile);
                break;
            case ToolSelection.WateringCan:
                Water(target_tile);
                break;
            case ToolSelection.CarrotSeed:
                Plant(target_tile, 0);
                break;
        }
        
    }

    private void Pull(Vector2I position) 
    {
        if(!Crops.Any(x => x.Key == position))
            return;

        var crop = Crops[position];

        switch (crop.Type) {
            case 0:
                CarrotCount += 1;
                break;
            case 3:
                TomatoCount += 1;
                break;
        }
    }

    private void Till(Vector2I position)
    {
        TillLayer.SetCell(position, 0, new Vector2I(6,0));

        var newDetails = new CropDetails {
            Type = -1,
            State = CropState.OnlyTilled,
            Watered = false,
            Atlas = null
        };

        Crops.Add(position, newDetails);
    }

    private void Plant(Vector2I position, int type)
    {
        if(!Crops.Any(x => x.Key == position))
            return;

        var crop = Crops[position];
        crop.Type = type;
        crop.Atlas = new Vector2I(10, type);

        Crops[position] = crop;

        GD.Print("Setting Cell for plant");

        CropLayer.SetCell(position, 0, new Vector2I(10, type));
    }

    private void Water(Vector2I position)
    {
        GD.Print("Entered Watering");

        var crop = Crops[position];
        if(crop.Watered)
            return;
        
        crop.Watered = true;
        Crops[position] = crop;
        var atlasCords = TillLayer.GetCellAtlasCoords(position);
        atlasCords.X += 1;

        GD.Print("Got Watering Texture");

        TillLayer.SetCell(position, 0, atlasCoords: atlasCords);
        GD.Print("Watered Plant");
    }

    private void Sleep() {
        foreach (var crop in Crops) {
            // Next Crop Size
            var nextState = GetNextCropState(crop.Value);
            CropLayer.SetCell(crop.Key, atlasCoords: nextState);

            // Unwater Land
            var till_atlas = TillLayer.GetCellAtlasCoords(crop.Key);
            till_atlas.X -= 1;
            TillLayer.SetCell(crop.Key, 0, atlasCoords: till_atlas);
        }
    } 

    // Convert crop info into next sprite
    private Vector2I GetNextCropState(CropDetails details)
    {
        if(!details.Watered)
            return new Vector2I(14, details.Type);

        switch (details.State) {
            case CropState.JustPlanted:
                return new Vector2I(11, details.Type);
            case CropState.StageTwo:
                return new Vector2I(12, details.Type);
            case CropState.StageThree:
                return new Vector2I(14, details.Type);
            case CropState.Ready:
                return new Vector2I(13, details.Type);
        }

        // Wilt if something bad happens
        return new Vector2I(13, details.Type);
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
                target_tile_pos = currentPosition += Vector2I.Right;
                break;
            default:
                // Impossible Case
                target_tile_pos = currentPosition;
                break;
        }

        return target_tile_pos;
    }

    
}
