/*
 * Marlow Greenan
 * Created: 11/01/2026
 * Last Updated: 11/01/2026
 * 
 * Controls a dialogue choice button.
 */
using MarUtility.Dialogue;
using MarUtility.UIExtensions;
using TMPro;
using UnityEngine;

public class DialogueChoiceUI : MonoBehaviour
{
    [SerializeField]
    private ObjectButton _button;
    [SerializeField]
    private TMP_Text _text;

    private DialogueManager dm;
    private int choiceIndex;

    #region GS
    public ObjectButton Button { get => _button; }
    public TMP_Text Text { get => _text; }
    #endregion

    public void Initialize(DialogueManager dm, int index)
    {
        this.dm = dm;
        choiceIndex = index;
    }

    public void SelectChoice()
        => dm.SelectChoice(choiceIndex);
}
