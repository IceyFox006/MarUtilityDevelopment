/*
 * Marlow Greenan
 * Created: 6/21/2026
 * Last Updated: 6/21/2026
 * 
 * Contains methods for converting variable types.
 */
using UnityEngine;

namespace MarUtility
{
    public static class Conversion
    {
        public static Vector3 ToVector3(Color c)
            => new Vector3(c.r, c.g, c.b);

        public static Color ToColor(Vector3 v, float alpha)
            => new Color(v.x, v.y, v.z, alpha);
    }
}


