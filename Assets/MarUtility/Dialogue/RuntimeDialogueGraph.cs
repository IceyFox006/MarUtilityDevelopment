/*
 * Marlow Greenan
 * Created: 10/25/2026
 * Last Updated: 10/27/2026
 * 
 * Contains data for dialogue usable in builds.
 */
using System;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "RuntimeDiallogueGraph", menuName = "Scriptable Objects/RuntimeDiallogueGraph")]
//=====================================================================================================================
// Runtime Dialogue Graph
//=====================================================================================================================
public class RuntimeDialogueGraph : ScriptableObject
{
    public string EntryNodeID;
    public List<RuntimeDialogueNode> Nodes = new List<RuntimeDialogueNode>();
}

//=====================================================================================================================
// Runtime Dialogue Node
//=====================================================================================================================
[Serializable]
public class RuntimeDialogueNode
{
    public string NodeID;
    public string SpeakerName;
    public string DialogueText;
    public string NextNodeID;
}
