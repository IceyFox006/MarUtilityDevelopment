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
        context.AddOutputPort("Out").Build();
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
        context.AddInputPort("In").Build();
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
        context.AddInputPort("In").Build();
        context.AddOutputPort("Out").Build();

        context.AddInputPort<string>("Speaker").Build();
        context.AddInputPort<string>("Dialogue").Build();
    }
}
