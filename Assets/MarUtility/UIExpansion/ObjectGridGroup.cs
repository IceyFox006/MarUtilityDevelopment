/*
 * Marlow Greenan
 * Created: 6/27/2026
 * Last Updated: 7/8/2026
 * 
 * Spaces out objects into rowns and/or columns.
 */
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace MarUtility.UIExtensions
{
    public class NewMonoBehaviourScript : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _children;

        [SerializeField, Tooltip("Number of rows and columns.\nX = column count.\nY = row count.")]
        private Vector3 _size = Vector3.one;
        [SerializeField, Tooltip("Size of children gameobjects.")]
        private Vector3 _childSize;
        [SerializeField, Tooltip("X = spacing between columns, Y = spacing between rows.")]
        private Vector3 _spacing;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < transform.childCount; i++)
                _children.Add(transform.GetChild(i).gameObject);

            SnapAll();
        }

        //Snaps all children to their grid positions.
        private void SnapAll()
        {
            for (int i = 0; i < _children.Count; i++)
            {
                if (i < _size.x * _size.y * _size.z) SnapChild(i);
                else transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void SnapChild(int i)
        {
            float x = transform.position.x + (i % _size.x) * (_spacing.x + _childSize.x);
            float y = -(transform.position.y + (int)(i / _size.y) * (_spacing.y + _childSize.y));//-(transform.position.y + (int)(i / _size.y) * (_spacing.y + _childSize.y));
            float z = transform.position.z + ((int)(i / (_size.x * _size.y)) * (_spacing.z + _childSize.z));
            _children[i].transform.position = new Vector3(x, y, z);
        }
    }
}


