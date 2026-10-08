using MarUtility;
using MarUtility.InspectorExtentions;
using NUnit.Framework;
using System.Collections.Generic;
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
        List<Transform> l = MarData.FindChildrenWithName(transform, "Hi");
        Debug.Log(l.Count);
    }
}
