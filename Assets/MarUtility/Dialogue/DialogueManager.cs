/*
 * Marlow Greenan
 * Created: 10/25/2026
 * Last Updated: 11/01/2026
 * 
 * Manages the dialogue progression.
 */
using MarUtility.ExecutionManagement;
using MarUtility.UIExtensions;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MarUtility.Dialogue
{
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
        [SerializeField, BoxGroup("UI")]
        private DialogueChoiceUI[] _choiceBoxes;

        [SerializeField]
        private ObjectEventSystem _oes;

        //SIMULATION
        [SerializeField, BoxGroup("Simulation")]
        private RuntimeDialogueGraph _simDialogue;

        //DATA
        private Dictionary<string, RuntimeDialogueNode> nodeDictionary = new Dictionary<string, RuntimeDialogueNode>();
        private RuntimeDialogueNode curNode;

        public override void Initialize()
        {
            for (int i = 0; i < _choiceBoxes.Count(); i++)
                _choiceBoxes[i].Initialize(this, i);
        }

        #region Dialogue
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

        //Shows the current node and its choices.
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
            _speakerNameText.text = curNode.Speaker.Name;

            if (!UpdateChoices())
                _oes.SwitchHover(_continueButton);
        }

        //Hides the dialogue panel, clears the node dictionary, and sets curNode to null.
        private void EndDialogue()
        {
            _dialoguePanel.SetActive(false);
            nodeDictionary.Clear();
            curNode = null;
        }
        #endregion

        #region Choice
        //Updates the choice buttons to match the current node.
        private bool UpdateChoices()
        {
            HideChoices();
            if (curNode.Choices.Count <= 0) return false;

            //Show choice boxes.
            for (int i = 0; i < curNode.Choices.Count; i++)
            {
                //Visibility
                _choiceBoxes[i].gameObject.SetActive(true);
                _choiceBoxes[i].Text.text = curNode.Choices[i].ChoiceText;

                //Navigation
                _choiceBoxes[i].Button.Navigation.Up = (MarData.InRange(i - 1, curNode.Choices.Count())) ? _choiceBoxes[i - 1].Button : null;
                _choiceBoxes[i].Button.Navigation.Down = (MarData.InRange(i + 1, curNode.Choices.Count())) ? _choiceBoxes[i + 1].Button : null;
            }
            _oes.SwitchHover(_choiceBoxes[0].GetComponent<ObjectButton>());
            return true;
        }

        //Deactivates all choice buttons.
        private void HideChoices()
        {
            foreach (DialogueChoiceUI dcUI in _choiceBoxes)
                dcUI.gameObject.SetActive(false);
        }

        public void SelectChoice(int i)
            => ShowNode(curNode.Choices[i].NextNodeID);
        #endregion

        #region Inspector
        [Button]
        private void StartDialogue()
            => StartDialogue(_simDialogue);
        #endregion
    }

}

