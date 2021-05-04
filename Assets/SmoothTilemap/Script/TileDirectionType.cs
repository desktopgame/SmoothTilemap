using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SmoothTilemap
{

    public enum TileDirectionType
    {
        LeftTop,
        Top,
        RightTop,
        Left,
        Center,
        Right,
        LeftBottom,
        Bottom,
        RightBottom,
    }

    public static class TileDirectionTypeExtension
    {
        public static Vector2Int ToVector2(this TileDirectionType self)
        {
            return self switch
            {
                TileDirectionType.LeftTop => Vector2Int.up + Vector2Int.left,
                TileDirectionType.Top => Vector2Int.up,
                TileDirectionType.RightTop => Vector2Int.up + Vector2Int.right,
                TileDirectionType.Left => Vector2Int.left,
                TileDirectionType.Center => Vector2Int.zero,
                TileDirectionType.Right => Vector2Int.right,
                TileDirectionType.LeftBottom => Vector2Int.down + Vector2Int.left,
                TileDirectionType.Bottom => Vector2Int.down,
                TileDirectionType.RightBottom => Vector2Int.down + Vector2Int.right,
                _ => throw new ArgumentException()
            };
        }
    }
}