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
using NUnit.Framework;
using System.ComponentModel;
using Unity.VisualScripting;

namespace MarUtility.Dialogue
{
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
    public class EndNode : Node
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("End").Build();
        }
    }

    //=================================================================================================================
    // Dialogue Node
    //=================================================================================================================
    [Serializable]
    public class DialogueNode : Node
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("Prev").Build();
            context.AddOutputPort("Next").Build();

            context.AddInputPort<SO_DialogueCharacter>("Speaker").Build();
            context.AddInputPort<string>("Dialogue").Build();
            context.AddInputPort<EDialogueEvent[]>("Events").Build();
        }
    }
    //=================================================================================================================
    // Choice Node
    //=================================================================================================================
    [Serializable]
    public class ChoiceNode : Node
    {
        const string choiceID = "Choice Count";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("Prev").Build();
            
            context.AddInputPort<SO_DialogueCharacter>("Speaker").Build();
            context.AddInputPort<string>("Dialogue").Build();
            context.AddInputPort<EDialogueEvent[]>("Events").Build();

            //Add multiple input and output ports for choices.
            var choice = GetNodeOptionByName(choiceID);
            choice.TryGetValue(out int portCount);
            for (int i = 0; i < portCount; i++)
            {
                context.AddInputPort<string>($"Choice Text {i}").Build();
                context.AddOutputPort($"Choice {i}").Build();
            }
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(choiceID);
        }
    }
    //=================================================================================================================
    // Relationship Node
    //=================================================================================================================
    [Serializable]
    public class RelationNode : Node
    {
        const string relationshipID = "Relation Count";
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("Prev").Build();
            context.AddOutputPort("Next").Build();

            context.AddInputPort<SO_DialogueCharacter>("Speaker").Build();
            context.AddInputPort<string>("Dialogue").Build();
            context.AddInputPort<EDialogueEvent[]>("Events").Build();

            //Add multiple input and output ports for choices.
            var relation = GetNodeOptionByName(relationshipID);
            relation.TryGetValue(out int portCount);
            for (int i = 0; i < portCount; i++)
            {
                context.AddInputPort<SO_DialogueCharacter>($"Character {i}").Build();
                context.AddInputPort<float>($"Point {i}").Build();
            }
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(relationshipID);
        }
    }
}

