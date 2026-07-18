/*
 * Marlow Greenan
 * Created: 6/21/2026
 * Last Updated: 6/21/2026
 * 
 * Contains methods for math related operations.
 */

using UnityEngine;

namespace MarUtility
{
    public static class Math
    {
        //Returns 1 if positive, -1 if negative.
        public static float BaseOne(float f)
        {
            if (f == 0) return 0;
            return (f > 0)? 1 : -1;
        }

        //If l is out of bounds of min or max, loop until its value is in bounds.
        public static float LoopAround(float l, float min, float max)
        {
            if (l > min && l < max) return l;
            return (l > max)? l - max : l + max;
        }

        public static Color RandomColor()
            => new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }
}

