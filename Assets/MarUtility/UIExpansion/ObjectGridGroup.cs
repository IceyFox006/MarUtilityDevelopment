/*
 * Marlow Greenan
 * Created: 6/27/2026
 * Last Updated: 7/18/2026
 * 
 * Spaces out objects into rowns and/or columns.
 */
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace MarUtility.UIExtensions
{
    public class NewMonoBehaviourScript : MonoBehaviour
    {
        //SIZING
        [SerializeField, BoxGroup("Sizing"), Tooltip("Number of rows and columns.\nX = column count.\nY = row count.")]
        private Vector3Int _size = Vector3Int.one;
        [SerializeField, BoxGroup("Sizing"), Tooltip("Size of children gameobjects.")]
        private Vector3 _childSize;
        [SerializeField, BoxGroup("Sizing"), Tooltip("X = spacing between columns, Y = spacing between rows.")]
        private Vector3 _spacing;

        //CHILDREN
        [SerializeField, BoxGroup("Children"), OnValueChanged("OnVC_SpawnChildren"), Tooltip("If TRUE, instead of using pre-existing children, spawn children in.\n*DELETES ALL PRE-EXISTING CHILDREN*")]
        private bool _spawnChildren;
        [SerializeField, BoxGroup("Children"), ShowIf("_spawnChildren")]
        private GameObject _childPrefab;
        [SerializeField, BoxGroup("Children"), HideIf("_spawnChildren"), OnValueChanged("OnVC_Children")]
        private List<GameObject> _children = new List<GameObject>();


        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            //Set up child list.
            if (!_spawnChildren) //Add pre-existing children to list.
                AddChildrenToList();
            else //Destroy pre-existing children, spawn children, then add to list.
            {
                DataMethod.DestroyChildren(transform);
                float spawnNum = _size.x * _size.y * _size.z;
                for (int i = 0; i < spawnNum; i++)
                {
                    GameObject clone = Instantiate(_childPrefab, transform);
                    _children.Add(clone);
                }
            }
            //Snap to positions.
            SnapAll();
        }

        private void AddChildrenToList()
        {
            _children.Clear();
            for (int i = 0; i < transform.childCount; i++)
                _children.Add(transform.GetChild(i).gameObject);
        }

        #region Alignment
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
        #endregion

        #region Inspector
        private void OnVC_SpawnChildren()
        {
            if (!_spawnChildren)
            {
                AddChildrenToList();
                OnVC_Children();
            }
        }

        private void OnVC_Children()
        {
            SnapAll();
        }
        #endregion
    }
}


