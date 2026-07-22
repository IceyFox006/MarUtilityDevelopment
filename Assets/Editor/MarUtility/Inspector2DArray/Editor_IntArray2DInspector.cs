/*
 * Marlow Greenan
 * Created: 7/21/2026
 * Last Updated: 7/21/2026 by Marlow Greenan
 * 
 * Allows int 2D arrays to be shown in the inspector.
 */
using UnityEditor;
using UnityEngine;

namespace MarUtility.InspectorExtentions
{
    [CustomPropertyDrawer(typeof(IntCellData))]
    public class Editor_IntArray2DInspector : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Draw array.
            EditorGUI.PrefixLabel(position, label);
            
            Rect newPosition = position;
            newPosition.y += 18f;
            SerializedProperty rows = property.FindPropertyRelative("rows");

            for (int x = 0; x < rows.arraySize; x++)
            {
                SerializedProperty row = rows.GetArrayElementAtIndex(x).FindPropertyRelative("row");

                newPosition.height = 20;
                newPosition.width = 30;

                for (int y = 0; y < row.arraySize; y++)
                {
                    EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(y), GUIContent.none);
                    newPosition.x += newPosition.width;
                }

                newPosition.x = position.x;
                newPosition.y += 20;
            }

            //Draw add row button.
            if (GUILayout.Button("+Row"))
            {
                rows.arraySize++;
            }

            //Draw remove row button.
            if (GUILayout.Button("-Row"))
            {
                if (rows.arraySize > 1)
                    rows.arraySize--;
            }

            //Draw add column button.
            if (GUILayout.Button("+Column"))
            {
                for (int y = 0; y < rows.arraySize; y++)
                {
                    SerializedProperty row = rows.GetArrayElementAtIndex(y).FindPropertyRelative("row");
                    row.arraySize++;
                }
            }

            //Draw remove column button.
            if (GUILayout.Button("-Column"))
            {
                SerializedProperty row = rows.GetArrayElementAtIndex(0).FindPropertyRelative("row");
                if (row.arraySize > 1)
                {
                    for (int y = 0; y < rows.arraySize; y++)
                    {
                        row = rows.GetArrayElementAtIndex(y).FindPropertyRelative("row");
                        row.arraySize--;
                    }
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty rows = property.FindPropertyRelative("rows");

            return (rows.arraySize * 2 * 12) + 5;
        }
    }
}

