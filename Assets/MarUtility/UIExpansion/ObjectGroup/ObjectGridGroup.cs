/*
 * Marlow Greenan
 * Created: 6/27/2026
 * Last Updated: 7/19/2026
 * 
 * Spaces out objects into rowns and/or columns.
 */
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MarUtility.UIExtensions
{
    public class ObjectGridGroup : MonoBehaviour
    {
        //ALIGNMENT
        [SerializeField, BoxGroup("Alignment"), OnValueChanged("OnVC_Children"), Tooltip("Number of rows and columns.\nX = column count.\nY = row count.")]
        private Vector3Int _size = Vector3Int.one;
        [SerializeField, BoxGroup("Alignment"), OnValueChanged("OnVC_Children"), Tooltip("Size of children gameobjects.")]
        private Vector3 _childSize;
        [SerializeField, BoxGroup("Alignment"), OnValueChanged("OnVC_Children"), Tooltip("X = spacing between columns, Y = spacing between rows.")]
        private Vector3 _spacing;

        //CHILDREN
        [SerializeField, BoxGroup("Children"), OnValueChanged("OnVC_SpawnChildren"), Tooltip("If TRUE, instead of using pre-existing children, spawn children in.\n*DELETES ALL PRE-EXISTING CHILDREN*")]
        private bool _spawnChildren;
        [SerializeField, BoxGroup("Children"), ShowIf("_spawnChildren"), ShowAssetPreview]
        private GameObject _childPrefab;
        [SerializeField, BoxGroup("Children"), HideIf("_spawnChildren"), OnValueChanged("OnVC_Children")]
        private List<GameObject> _children = new List<GameObject>(); //Used to create grid, NEVER USED AFTER.

        //ENTERANCE/EXIT
        [SerializeField, BoxGroup("Entrance/Exit")]
        private float _intervalBetweenPiece = 0.01f;
        [SerializeField, BoxGroup("Entrance/Exit")]
        private FrontBack _entranceListExecutionOrder;
        [SerializeField, BoxGroup("Entrance/Exit")]
        private FrontBack _exitListExecutionOrder;

        //SWAP
        [SerializeField, Label("Swap Lerp Data"), BoxGroup("Swap")]
        private LerpData _swapLD;

        //SIMULATE
        [SerializeField, BoxGroup("Simulate")]
        private Vector3Int _simSwapCoordA;
        [SerializeField, BoxGroup("Simulate")]
        private Vector3Int _simSwapCoordB;
        [SerializeField, BoxGroup("Simulate"),Range(0, 1)]
        private float _simMovePercent;

        private ChildData[,,] grid;
        private ObjectGroupChild[] _groupChildren;


        private void Awake()
        {
            Initialize();
        }

        //Sets up children list, creates grid, snaps all children, and initializes group children.
        private void Initialize()
        {
            //Set up child list.
            if (!_spawnChildren) //Add pre-existing children to list.
                AddChildrenToList();
            else //Destroy pre-existing children, spawn children, then add to list.
            {
                _children.Clear();
                DataMethod.DestroyChildren(transform);
                float spawnNum = _size.x * _size.y * _size.z;
                for (int i = 0; i < spawnNum; i++)
                {
                    GameObject clone = Instantiate(_childPrefab, transform);
                    _children.Add(clone);
                }
            }

            //Create grid.
            CreateGrid();

            //Snap to positions.
            SnapAll();

            //Initialize object group children.
            SetGroupChildrenList();
        }

        #region Movement
        public void PlayReturnToOrigin()
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    for (int z = 0; z < grid.GetLength(2); z++)
                        grid[x, y, z].Ogc.BeginReturnToOriginLerp();
                }
            }
        }

        //Plays entrance movement for all children.
        public IEnumerator PlayEntrance()
        {
            if (_entranceListExecutionOrder == FrontBack.FRONT) //Front
            {
                for (int z = 0; z < grid.GetLength(2); z++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        for (int x = 0; x < grid.GetLength(0); x++)
                        {
                            grid[x, y, z].Ogc.BeginEntranceLerp();
                            yield return new WaitForSeconds(_intervalBetweenPiece);
                        }
                    }
                }
            }
            else //Back
            {
                for (int z = grid.GetLength(2) - 1; z >= 0 ; z--)
                {
                    for (int y = grid.GetLength(1) - 1; y >= 0; y--)
                    {
                        for (int x = grid.GetLength(0) - 1; x >= 0; x--)
                        {
                            grid[x, y, z].Ogc.BeginEntranceLerp();
                            yield return new WaitForSeconds(_intervalBetweenPiece);
                        }
                    }
                }
            }
        }

        //Plays exit movement for all children.
        public IEnumerator PlayExit()
        {
            if (_exitListExecutionOrder == FrontBack.FRONT) //Front
            {
                for (int z = 0; z < grid.GetLength(2); z++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        for (int x = 0; x < grid.GetLength(0); x++)
                        {
                            grid[x, y, z].Ogc.BeginExitLerp();
                            yield return new WaitForSeconds(_intervalBetweenPiece);
                        }
                    }
                }
            }
            else //Back
            {
                for (int z = grid.GetLength(2) - 1; z >= 0; z--)
                {
                    for (int y = grid.GetLength(1) - 1; y >= 0; y--)
                    {
                        for (int x = grid.GetLength(0) - 1; x >= 0; x--)
                        {
                            grid[x, y, z].Ogc.BeginExitLerp();
                            yield return new WaitForSeconds(_intervalBetweenPiece);
                        }
                    }
                }
            }
        }

        //Plays swap movement and swaps the data.
        public bool PlaySwap(Vector3Int coordA, Vector3Int coordB)
        {
            ObjectGroupChild objA;
            ObjectGroupChild objB;

            if (CanMoveCoord(coordA, out objA) && CanMoveCoord(coordB, out objB))
            {

                //Swap visuals.
                objA.BeginPositionLerp(_swapLD, objB.OriginPos);
                objB.BeginPositionLerp(_swapLD, objA.OriginPos);

                //Swap data.
                StartCoroutine(SwapCD(coordA, coordB, _swapLD.Duration));

                return true;
            }
            else
                return false;
        }

        //Moves coordMoving object partially towards coordEnd object.
        public bool PlayPercentMoveTowards(Vector3Int coordMoving, Vector3Int coordEnd, float percent)
        {
            ObjectGroupChild objA;
            ObjectGroupChild objB;

            if (CanMoveCoord(coordMoving, out objA) && CanMoveCoord(coordEnd, out objB))
            {
                Vector3 lEnd = Vector3.Lerp(objA.OriginPos, objB.OriginPos, percent);

                //Swap visuals.
                objA.BeginPositionLerp(_swapLD, lEnd);

                return true;
            }
            else
                return false;
        }

        //Swaps data after visual swap has been completed.
        private IEnumerator SwapCD(Vector3Int coordA, Vector3Int coordB, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            SwapChildrenData(coordA, coordB);
        }
        #endregion

        #region Data
        //Clears _children and adds transform's children to _children.
        private void AddChildrenToList()
        {
            _children.Clear();
            for (int i = 0; i < transform.childCount; i++)
                _children.Add(transform.GetChild(i).gameObject);
        }

        private void SetGroupChildrenList()
        {
            _groupChildren = GetComponentsInChildren<ObjectGroupChild>();
            foreach (ObjectGroupChild child in _groupChildren)
                child.Initialize();
        }

        //Creates a grid out of _children.
        private void CreateGrid()
        {
            grid = new ChildData[_size.x, _size.y, _size.z];

            int x = 0;
            int y = -1;
            int z = 0;
            for (int i = 0; i < _children.Count; i++)
            {
                if (i < _size.x * _size.y * _size.z)
                {
                    _children[i].SetActive(true);

                    x = i % _size.x; //x
                    if (x == 0) y++; //y

                    //z = ;

                    grid[x, y, z] = new ChildData(_children[i]);
                }
                else
                    _children[i].SetActive(false);
            }
        }

        //Swaps only the DATA of the gameObjects at coordA and coordB. NO EFFECT ON IN GAME POSITION.
        public void SwapChildrenData(Vector3Int coordA, Vector3Int coordB)
        {
            ChildData holder = GetDataAtCoord(coordA);
            SetDataAtCoord(coordA, GetDataAtCoord(coordB));
            SetDataAtCoord(coordB, holder);
        }

        //Gets the object at coord.
        private ChildData GetDataAtCoord(Vector3Int coord)
            => grid[coord.x, coord.y, coord.z];

        //Sets object at coords and recalculates originPos.
        private void SetDataAtCoord(Vector3Int coord, ChildData newObj)
        {
            grid[coord.x, coord.y, coord.z] = newObj;
            
            ObjectGroupChild child = newObj.Ogc;
            if (child != null)
                child.OriginPos = CalculateOriginPos(coord);
        }

        //Returns true if coord is in the grid bounds.
        private bool InBounds(Vector3Int coord)
        {
            if (!(coord.x >= 0 && coord.x <= grid.GetLength(0))) return false;
            if (!(coord.y >= 0 && coord.y <= grid.GetLength(1))) return false;
            if (!(coord.z >= 0 && coord.z <= grid.GetLength(2))) return false;
            return true;
        }

        //Returns true if coord is in bounds, not null, and is not moving.
        private bool CanMoveCoord(Vector3Int coord, out ObjectGroupChild ogc)
        {
            ogc = null;

            if (!(InBounds(coord))) return false;

            ogc = GetDataAtCoord(coord).Ogc;

            if (ogc == null) return false;
            if (ogc.IsLerping) return false;

            return true;
        }
        #endregion

        #region Alignment
        //Snaps all children to their grid positions.
        private void SnapAll()
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    for (int z = 0; z < grid.GetLength(2); z++)
                        SnapChild(new Vector3Int(x, y, z));
                }
            }
        }

        private Vector3 CalculateOriginPos(Vector3Int coord)
        {
            if (GetDataAtCoord(coord) == null) return new Vector3(-1, -1, -1);

            float xPos = transform.position.x + (coord.x * (_spacing.x + _childSize.x));
            float yPos = transform.position.y - (coord.y * (_spacing.y + _childSize.y));
            float zPos = transform.position.z + (coord.z * (_spacing.z + _childSize.z));

            return new Vector3(xPos, yPos, zPos);
        }

        //Snaps a child to their origin position.
        private void SnapChild(Vector3Int coord)
        {
            if (GetDataAtCoord(coord) == null) return;

            grid[coord.x, coord.y, coord.z].Obj.transform.position = CalculateOriginPos(coord);
        }
        #endregion

        #region Inspector
        private void OnVC_SpawnChildren()
        {
            if (_spawnChildren) return;

            AddChildrenToList();
            OnVC_Children();
        }

        private void OnVC_Children()
        {
            CreateGrid();
            SnapAll();
        }

        [Button]
        private void SimulateEntrance()
        {
            if (!EditorApplication.isPlaying)
            {
                DebugMessages.SimulationPlaytestOnly("Entrance");
                return;
            }
            StartCoroutine(PlayEntrance());
        }

        [Button]
        private void SimulateExit()
        {
            if (!EditorApplication.isPlaying)
            {
                DebugMessages.SimulationPlaytestOnly("Exit");
                return;
            }
            StartCoroutine(PlayExit());
        }

        [Button]
        private void SimulateSwap()
        {
            if (!EditorApplication.isPlaying)
            {
                DebugMessages.SimulationPlaytestOnly("Swap");
                return;
            }
            PlaySwap(_simSwapCoordA, _simSwapCoordB);
        }

        [Button]
        private void SimulatePercentMoveTowards()
        {
            if (!EditorApplication.isPlaying)
            {
                DebugMessages.SimulationPlaytestOnly("Percent Move Towards");
                return;
            }
            PlayPercentMoveTowards(_simSwapCoordA, _simSwapCoordB, _simMovePercent);
        }

        [Button]
        private void SimulateReturnToOrigin()
        {
            if (!EditorApplication.isPlaying)
            {
                DebugMessages.SimulationPlaytestOnly("Percent Move Towards");
                return;
            }
            PlayReturnToOrigin();
        }
        #endregion

        //=============================================================================================================
        private class ChildData
        {
            private GameObject obj;
            private ObjectGroupChild ogc;

            public ChildData(GameObject go)
            {
                obj = go;
                ogc = go.GetComponent<ObjectGroupChild>();
            }

            #region GS
            public GameObject Obj { get => obj; set => obj = value; }
            public ObjectGroupChild Ogc { get => ogc; set => ogc = value; }
            #endregion
        }
    }
}


