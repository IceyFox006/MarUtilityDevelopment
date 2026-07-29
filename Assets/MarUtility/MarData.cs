/*
 * Marlow Greenan
 * Created: 4/19/2026
 * Last Updated: 7/27/2026
 * 
 * Contains various reuable enums.
 */
using System;
using UnityEngine;

namespace MarUtility
{
    public class MarData 
    {
        //DIRECTIONS
        private static Vector2[] direction2DVec2 = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };
        private static Vector2Int[] direction2DVec2Int = { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

        private static Vector3[] direction2DVec3 = { Vector3.up, Vector3.down, Vector3.right, Vector3.left };
        private static Vector3Int[] direction2DVec3Int = { Vector3Int.up, Vector3Int.down, Vector3Int.right, Vector3Int.left };

        private static Vector3[] direction3DVec3 = { Vector3.up, Vector3.down, Vector3.right, Vector3.left, Vector3.forward, Vector3.back };
        private static Vector3Int[] direction3DVec3Int = { Vector3Int.up, Vector3Int.down, Vector3Int.right, Vector3Int.left, Vector3Int.forward, Vector3Int.back };

        #region GS
        public static Vector2[] Direction2DVec2 { get => direction2DVec2; }
        public static Vector2Int[] Direction2DVec2Int { get => direction2DVec2Int; }
        public static Vector3[] Direction2DVec3 { get => direction2DVec3; }
        public static Vector3Int[] Direction2DVec3Int { get => direction2DVec3Int; }
        public static Vector3[] Direction3DVec3 { get => direction3DVec3; }
        public static Vector3Int[] Direction3DVec3Int { get => direction3DVec3Int; }
        #endregion
    }
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        FORWARD,
        BACKWARD,
    }
    public enum FrontBack
    {
        FRONT,
        BACK,
    }

    public enum Dimension
    {
        _2D,
        _3D,
    }

    public enum SetValue
    {
        NULL,
        THIS,
    }
}

