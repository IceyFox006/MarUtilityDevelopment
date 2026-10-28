/*
 * Marlow Greenan
 * Created: 10/25/2026
 * Last Updated: 10/27/2026
 * 
 * Manages the dialogue progression.
 */
using MarUtility.ExecutionManagement;
using MarUtility.UIExtensions;
using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Manager
{
    //UI
    [SerializeField, BoxGroup("UI")]
    private GameObject _dialoguePanel;
    [SerializeField, BoxGroup("UI")]
    private TMP_Text _speakerNameText;
    [SerializeField, BoxGroup("UI")]
    private TMP_Text _dialogueText;
    [SerializeField, BoxGroup("UI")]
    private ObjectButton _continueButton;

    [SerializeField]
    private ObjectEventSystem _oes;

    //SIMULATION
    [SerializeField, BoxGroup("Simulation")]
    private RuntimeDialogueGraph _simDialogue;

    //DATA
    private Dictionary<string, RuntimeDialogueNode> nodeDictionary = new Dictionary<string, RuntimeDialogueNode>();
    private RuntimeDialogueNode curNode;

    public void StartDialogue(RuntimeDialogueGraph rtDG)
    {
        //If already in dialogue, exit.
        if (curNode != null)
        {
            Debug.LogError("Cannot start dialogue, when already in dialogue.");
            return;
        }

        //Set-up node directory.
        foreach (var node in rtDG.Nodes)
            nodeDictionary[node.NodeID] = node;

        //Show the dialogue panel and first node.
        _dialoguePanel.SetActive(true);
        _oes.SwitchHover(_continueButton);
        ShowNode(rtDG.EntryNodeID);
    }

    //Shows the next node.
    public void NextNode()
        => ShowNode(curNode.NextNodeID);

    private void ShowNode(string nodeID)
    {
        //Node dicitonary does not have the nodeID within, end the dialogue.
        if (!nodeDictionary.ContainsKey(nodeID) || string.IsNullOrEmpty(nodeID))
        {
            EndDialogue();
            return;
        }

        curNode = nodeDictionary[nodeID];
        _dialogueText.text = curNode.DialogueText;
    }

    //Hides the dialogue panel, clears the node dictionary, and sets curNode to null.
    private void EndDialogue()
    {
        _dialoguePanel.SetActive(false);
        nodeDictionary.Clear();
        curNode = null;
    }

    #region Inspector
    [Button]
    private void StartDialogue()
        => StartDialogue(_simDialogue);
    #endregion
}
