/*
 * Marlow Greenan
 * Created: 10/25/2026
 * Last Updated: 11/01/2026
 * 
 * Contains data for dialogue usable in builds.
 */
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarUtility.Dialogue
{
    //=================================================================================================================
    // Runtime Dialogue Graph
    //=================================================================================================================
    public class RuntimeDialogueGraph : ScriptableObject
    {
        public string EntryNodeID;
        public List<RuntimeDialogueNode> Nodes = new List<RuntimeDialogueNode>();
    }

    //=================================================================================================================
    // Runtime Dialogue Node
    //=================================================================================================================
    [Serializable]
    public class RuntimeDialogueNode
    {
        public string NodeID;
        public string NextNodeID;

        public SO_DialogueCharacter Speaker;
        public string DialogueText;
        public EDialogueEvent[] Events;

        public List<ChoiceData> Choices = new List<ChoiceData>();
        public List<RelationData> Relations = new List<RelationData>();
    }
    //=================================================================================================================
    // Choice Data
    //=================================================================================================================
    [Serializable]
    public class ChoiceData
    {
        public string ChoiceText;
        public string NextNodeID;
    }
    //=================================================================================================================
    // Relation Data
    //=================================================================================================================
    [Serializable]
    public class RelationData
    {
        public SO_DialogueCharacter Character;
        public float Point;
    }
}
