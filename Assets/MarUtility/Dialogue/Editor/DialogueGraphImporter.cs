/*
 * Marlow Greenan
 * Created: 10/25/2026
 * Last Updated: 10/27/2026
 * 
 * Transfers data from the Unity editor graph into a runtime scriptable object for builds.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;


[ScriptedImporter(1, DialogueGraph.AssetExtension)]
public class DialogueGraphImporter : ScriptedImporter
{
    //Goes off when the graph is saved. Transfers all the data from the editor graph to a runtime scriptable object for use in builds.
    public override void OnImportAsset(AssetImportContext ctx)
    {
        DialogueGraph editorGraph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);
        RuntimeDialogueGraph runtimeGraph = ScriptableObject.CreateInstance<RuntimeDialogueGraph>();
        var nodeIDMap = new Dictionary<INode, string>();

        //Assigns unique node ids.
        foreach (var node in editorGraph.GetNodes())
            nodeIDMap[node] = Guid.NewGuid().ToString();

        //Sets start nodes.
        var startNode = editorGraph.GetNodes().OfType<StartNode>().FirstOrDefault();
        if (startNode != null)
        {
            //Sets first dialogue node connected to the start point.
            var entryPoint = startNode.GetOutputPorts().FirstOrDefault()?.FirstConnectedPort;
            if (entryPoint != null)
                runtimeGraph.EntryNodeID = nodeIDMap[entryPoint.GetNode()];
        }

        foreach (var node in editorGraph.GetNodes())
        {
            if (node is StartNode || node is EndNode) continue; //Skip start and end nodes.

            //Save the editor dialogue node data to a runtime node.
            var runtimeNode = new RuntimeDialogueNode { NodeID = nodeIDMap[node] };
            if (node is DialogueNode dialogueNode)
                ProcessDialogueNode(dialogueNode, runtimeNode, nodeIDMap);

            runtimeGraph.Nodes.Add(runtimeNode);
        }
        ctx.AddObjectToAsset("RuntimeData", runtimeGraph);
        ctx.SetMainObject(runtimeGraph);
    }

    //Converts the data of a single node.
    private void ProcessDialogueNode(DialogueNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap)
    {
        //Get data from editor node and assign it to runtime node.
        runtimeNode.SpeakerName = GetPortValue<string>(node.GetInputPortByName("Speaker"));
        runtimeNode.DialogueText = GetPortValue<string>(node.GetInputPortByName("Dialogue"));

        //Go to next node.
        var nextNodePort = node.GetOutputPortByName("Next").FirstConnectedPort;
        if (nextNodePort != null)
            runtimeNode.NextNodeID = nodeIDMap[nextNodePort.GetNode()];
    }

    //Returns the value of the port.
    private T GetPortValue<T>(IPort port)
    {
        if (port == null) return default;

        if (port.FirstConnectedPort.GetNode() is IVariableNode variableNode)
        {
            variableNode.Variable.TryGetDefaultValue(out T value);
            return value;
        }

        port.TryGetValue(out T fallbackValue);
        return fallbackValue;
    }
}
