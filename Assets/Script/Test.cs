using MarUtility;
using MarUtility.InspectorExtentions;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private IntCellData _intGrid;

    [SerializeField]
    private BoolCellData _boolGrid;

    [SerializeField]
    private LerpPositionDataEndP t;

    private void Start()
    {
        if (MarData.FindChildWithName(transform, "Hi") != null)
            Debug.Log("Found");
        else
            Debug.Log("Not found");
    }
}
