/*
 * Marlow Greenan
 * Created: 10/25/2026
 * Last Updated: 10/27/2026
 * 
 * Contains data for dialogue nodes.
 */
using UnityEngine;
using Unity.GraphToolkit.Editor;
using System;

public class DialogueNodes { }
//=====================================================================================================================
// Start Node
//=====================================================================================================================
[Serializable]
public class StartNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddOutputPort("Begin").Build();
    }
}

//=====================================================================================================================
// End Node
//=====================================================================================================================
[Serializable]
public class EndNode: Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("End").Build();
    }
}

//=====================================================================================================================
// Dialogue Node
//=====================================================================================================================
[Serializable]
public class DialogueNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("Previous").Build();
        context.AddOutputPort("Next").Build();

        context.AddInputPort<string>("Speaker").Build();
        context.AddInputPort<string>("Dialogue").Build();
    }
}
