/*
 * Marlow Greenan
 * Created: 7/21/2026
 * Last Updated: 7/21/2026 by Marlow Greenan
 * 
 * Allows 2D arrays to be shown in the inspector.
 */
using UnityEditor;
using UnityEngine;

namespace MarUtility.InspectorExtentions
{
    [CustomPropertyDrawer(typeof(IntCellData))]
    public class Editor_Array2DInspector : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PrefixLabel(position, label);
            
            Rect newPosition = position;
            newPosition.y += 18f;
            SerializedProperty rows = property.FindPropertyRelative("rows");

            for (int i = 0; i < rows.arraySize; i++)
            {
                SerializedProperty row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("row");

                newPosition.height = 20;
                newPosition.width = 30;

                for (int j = 0; j < row.arraySize; j++)
                {
                    EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(j), GUIContent.none);
                    newPosition.x += newPosition.width;
                }

                newPosition.x = position.x;
                newPosition.y += 20;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty rows = property.FindPropertyRelative("rows");

            return rows.arraySize * 2 * 12;
        }
    }
}

