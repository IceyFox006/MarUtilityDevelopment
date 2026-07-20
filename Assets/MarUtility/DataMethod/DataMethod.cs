/*
 * Marlow Greenan
 * Created: 7/9/2026
 * Last Updated: 7/9/2026
 * 
 * Spaces out objects into rowns and/or columns.
 */
using UnityEngine;

namespace MarUtility
{
    public class DataMethod : MonoBehaviour
    {
        //Destroys all children of the parent.
        public static void DestroyChildren(Transform parent)
        {
            for (int i = parent.childCount - 1; i > -1; i--)
                Destroy(parent.GetChild(i).gameObject);
        }
    }
}

