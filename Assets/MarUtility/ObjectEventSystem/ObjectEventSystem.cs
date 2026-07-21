/*
 * Marlow Greenan
 * Created: 4/19/2026
 * Last Updated: 6/21/2026 by Marlow Greenan
 * 
 * Runs the system for object buttons.
 */

using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility.ObjectEventSystem
{
    public class ObjectEventSystem : ExecutionManagement.Manager
    {
        //SELECTION
        [SerializeField, BoxGroup("Selection")]
        private ObjectButton _firstSelected;
        [SerializeField, BoxGroup("Selection"), MinValue(0), OnValueChanged("OnVC_IndexReplaced"), Tooltip("The max number of buttons that can be selected at once.")]
        private int _maxNumSelected = 1;
        //Confirm on Select
        [SerializeField, BoxGroup("Selection"), ShowIf("ShowConfirmOnSelect")]
        private bool _confirmOnSelect = false;
        //Replace Select
        [SerializeField, BoxGroup("Selection"), Tooltip("Instead of preventing selection, deselects one of the selected buttons, to select the curHover on select.")]
        private bool _replaceSelection = false;
        [SerializeField, BoxGroup("Selection"), MinValue(0), ShowIf("_replaceSelection"), OnValueChanged("OnVC_IndexReplaced"), Tooltip("The index of the button that will be deselected.")]
        private int _indexReplaced = 0;
        //Deselect Confirm
        [SerializeField, BoxGroup("Selection"), Tooltip("When confirmed, all selected buttons will be deselected.")]
        private bool _deselectOnConfirm = true;

        [ShowNonSerializedField]
        private ObjectButton curHover = null;
        private List<ObjectButton> curSelected = new List<ObjectButton>();

        //INPUT
        [SerializeField, BoxGroup("Input")]
        private bool _receiveInput = true;
        [SerializeField, BoxGroup("Input")]
        private InputActionAsset _inputActions;
        //Move
        [SerializeField, BoxGroup("Input")]
        private string moveActionPath = "MOVE";
        private InputAction move;
        private Vector2 moveDirection;
        //Select
        [SerializeField, BoxGroup("Input")]
        private string selectActionPath = "SELECT";
        private InputAction select;
        //Confirm
        [SerializeField, BoxGroup("Input")]
        private string confirmActionPath = "CONFIRM";
        private InputAction confirm;

        #region GS
        public bool ReceiveInput
        {
            get => _receiveInput;
            set
            {
                _receiveInput = value;
                if (_receiveInput) EnableInput(); else DisableInput();
            }
        }

        public int IndexReplaced
        {
            get => _indexReplaced;
            set
            {
                _indexReplaced = value;
                OnVC_IndexReplaced();
            }
        }

        public bool ConfirmOnSelect
        {
            get => _confirmOnSelect;
            set
            {
                _confirmOnSelect = value;
                ShowConfirmOnSelect();
            }
        }
        #endregion

        private void OnDestroy()
        {
            DisableInput();
        }
        public override void Initialize()
        {
            curHover = _firstSelected;
            if (curHover != null)
                _firstSelected.OnHoverEnter();

            InitializeInput();
            if (_receiveInput)
                EnableInput();
        }

        #region Input
        //Assigns actions to inputs.
        private void InitializeInput()
        {
            _inputActions.Enable();
            move = _inputActions.FindAction(moveActionPath);
            select = _inputActions.FindAction(selectActionPath);
            confirm = _inputActions.FindAction(confirmActionPath);
        }

        //Add input listeners.
        private void EnableInput()
        {
            move.performed += Move_performed;
            select.performed += Select_performed;
            confirm.performed += Confirm_performed;
        }

        //Remove input listeners.
        private void DisableInput()
        {
            move.performed -= Move_performed;
            select.performed -= Select_performed;
            confirm.performed -= Confirm_performed;
        }

        //Switches hover to button in direction.
        private void Move_performed(InputAction.CallbackContext obj)
        {
            moveDirection = move.ReadValue<Vector2>();

            //Switch Hover
            if (moveDirection == Vector2.up && CanMoveTo(curHover.Navigation.Up)) //Up
                SwitchHover(curHover.Navigation.Up);
            else if (moveDirection == Vector2.down && CanMoveTo(curHover.Navigation.Down)) //Down
                SwitchHover(curHover.Navigation.Down);
            else if (moveDirection == Vector2.left && CanMoveTo(curHover.Navigation.Left)) //Left
                SwitchHover(curHover.Navigation.Left);
            else if (moveDirection == Vector2.right && CanMoveTo(curHover.Navigation.Right)) //Right
                SwitchHover(curHover.Navigation.Right);
        }

        //Select if button is not already selected, deselect if it is.
        private void Select_performed(InputAction.CallbackContext obj)
        {
            if (curHover.IsSelected) //Deselect if selected.
            {
                RemoveSelected(curHover);
                return; //Deselected piece.
            }

            if (curSelected.Count < _maxNumSelected) //Select if there is room.
            {
                AddSelected(curHover);

                if (_confirmOnSelect)
                    ConfirmSelected();
            }
            else
            {
                if (_replaceSelection)
                {
                    RemoveSelected(curSelected[_indexReplaced]);
                    AddSelected(curHover);
                }
            }
        }

        //Confirm button.
        private void Confirm_performed(InputAction.CallbackContext obj)
        {
            ConfirmSelected();
        }
        #endregion

        #region Selection Management
        //Switches which button is currently being hovered over.
        public void SwitchHover(ObjectButton ob)
        {
            if (curHover != null)
                curHover.OnHoverExit();

            curHover = ob;

            if (curHover != null)
                curHover.OnHoverEnter();
        }

        //Adds ob to curSelected and selects it.
        private void AddSelected(ObjectButton ob)
        {
            ob.OnSelect();
            curSelected.Add(ob);
        }

        //Removes ob from curSelected and deselects it.
        private ObjectButton RemoveSelected(ObjectButton ob)
        {
            ob.OnDeselect();
            curSelected.Remove(ob);
            return ob;
        }

        //Invokes confirm on all selected buttons.
        private void ConfirmSelected()
        {
            for (int i = curSelected.Count - 1; i >= 0; i--)
            {
                if (_deselectOnConfirm)
                    RemoveSelected(curSelected[i]).OnConfirm();
                else
                    curSelected[i].OnConfirm();
            }
        }
        #endregion

        #region Check
        //Returns true if bo can be moved to.
        private bool CanMoveTo(ObjectButton bo)
                => (bo != null && bo.Interactable);
        #endregion

        #region Inspector
        private void OnVC_IndexReplaced()
        {
            if (_indexReplaced >= _maxNumSelected)
                _indexReplaced = _maxNumSelected;
        }

        private bool ShowConfirmOnSelect()
        {
            if (_maxNumSelected == 1)
                return true;
            else
            {
                _confirmOnSelect = false;
                return false;
            }
        }
        #endregion
    }
}

