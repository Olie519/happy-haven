using System.Collections.Generic;
using Godot;


namespace HelpingHaven;

public static class Carrot 
{
    public static Vector2I? GetSpriteForState(CropState state) 
    {
        switch (state) {
            case CropState.JustPlanted:
                return new Vector2I(10 ,0);
            case CropState.StageTwo:
                return new Vector2I(11, 0);
            case CropState.StageThree:
                return new Vector2I(12 ,0);
            case CropState.Ready:
                return new Vector2I(14, 0);
            case CropState.Wilted:
                return new Vector2I(13, 0);
        }

        return null;
    }
}