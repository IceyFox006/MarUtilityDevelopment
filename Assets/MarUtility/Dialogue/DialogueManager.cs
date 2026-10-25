using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public RuntimeDialogueGraph RuntimeDialogueGraph;

    public GameObject DialoguePanel;
    public TMP_Text SpeakerNameText;
    public TMP_Text DialogueText;

    private Dictionary<string, RuntimeDialogueNode> nodeDictionary = new Dictionary<string, RuntimeDialogueNode>();
    private RuntimeDialogueNode curNode;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        foreach (var node in RuntimeDialogueGraph.Nodes)
        {
            nodeDictionary[node.NodeID] = node;
        }

        if (!string.IsNullOrEmpty(RuntimeDialogueGraph.EntryNodeID))
            ShowNode(RuntimeDialogueGraph.EntryNodeID);
        else
            EndDialogue();
    }

    public void NextNode()
    {
        if (string.IsNullOrEmpty(curNode.NextNodeID))
            ShowNode(curNode.NextNodeID);
        else
            EndDialogue();
    }

    private void ShowNode(string nodeID)
    {
        //Node dicitonary does not have the nodeID within in.
        if (!nodeDictionary.ContainsKey(nodeID))
        {
            EndDialogue();
            return;
        }

        curNode = nodeDictionary[nodeID];
        DialoguePanel.SetActive(true);
        DialogueText.text = curNode.DialogueText;
    }

    private void EndDialogue()
    {
        DialoguePanel.SetActive(false);
        curNode = null;
    }
}
