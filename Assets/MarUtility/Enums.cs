/*
 * Marlow Greenan
 * Created: 4/19/2026
 * Last Updated: 7/19/2026
 * 
 * Contains various reuable enums.
 */
using System;

namespace MarUtility
{
    public class Enums { }
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

    [Flags]
    public enum LerpEvent
    {
        NONE = 0 << 000,
        START = 1 << 100,
        BODY = 1 << 200,
        END = 1 << 300,
    }

}

