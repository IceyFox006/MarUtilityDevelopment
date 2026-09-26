/*
 * Marlow Greenan
 * Created: 4/19/2026
 * Last Updated: 9/15/2026 by Marlow Greenan
 * 
 * Runs the system for object buttons.
 */

using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility.UIExtensions
{
    public class ObjectEventSystem : ExecutionManagement.Manager
    {
        //SELECTION
        [SerializeField, BoxGroup("Selection")]
        private ObjectButton _firstSelected;
        [SerializeField, BoxGroup("Selection"), MinValue(0), OnValueChanged("OnVC_IndexReplaced"), Tooltip("The max number of buttons that can be selected at once.")]
        private int _maxNumSelected = 1;
        //Confirm on Select
        [SerializeField, BoxGroup("Selection"), Tooltip("Confirms when the max number of selected objects has been reached.")]//ShowIf("ShowConfirmOnSelect")]
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
        [SerializeField]
        private string _possessedPlayerID;
        [SerializeField, BoxGroup("Input")]
        private bool _receiveInput = true;
        [SerializeField, BoxGroup("Input"), Tooltip("The direction that an input leads to.\nLeave as null for normal directions.")]
        private DistortedMoveInput _curMoveInputDistortion;
        [SerializeField, BoxGroup("Input")]
        private PlayerInput _playerInput;
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

        public PlayerInput PlayerInput { get => _playerInput; set => _playerInput = value; }
        public string PossessedPlayerID { get => _possessedPlayerID; set => _possessedPlayerID = value; }
        public DistortedMoveInput CurMoveInputDistortion { get => _curMoveInputDistortion; set => _curMoveInputDistortion = value; }
        #endregion

        private void OnDestroy()
        {
            DisableInput();
        }
        public override void Initialize()
        {
            SwitchHover(_firstSelected);

            InitializeInput();
            if (_receiveInput)
                EnableInput();
        }

        #region Input
        //Assigns actions to inputs.
        public void InitializeInput()
        {
            if (_playerInput == null) return;

            _playerInput.actions.Enable();
            move = _playerInput.actions.FindAction(moveActionPath);
            select = _playerInput.actions.FindAction(selectActionPath);
            confirm = _playerInput.actions.FindAction(confirmActionPath);
        }

        //Add input listeners.
        private void EnableInput()
        {
            if (_playerInput == null) return;

            move.performed += Move_performed;
            select.performed += Select_performed;
            confirm.performed += Confirm_performed;
        }

        //Remove input listeners.
        private void DisableInput()
        {
            if (_playerInput == null) return;

            move.performed -= Move_performed;
            select.performed -= Select_performed;
            confirm.performed -= Confirm_performed;
        }

        //Updates move direction with the input action value and if there is a distortion, applies it.
        private void LinkMoveDirection()
        {
            moveDirection = move.ReadValue<Vector2>();

            if (_curMoveInputDistortion != null)
            {
                switch (moveDirection)
                {
                    case Vector2 v when v.Equals(Vector2.up): moveDirection = MarData.ToVector2(_curMoveInputDistortion.UpPath); break;
                    case Vector2 v when v.Equals(Vector2.down): moveDirection = MarData.ToVector2(_curMoveInputDistortion.DownPath); break;
                    case Vector2 v when v.Equals(Vector2.left): moveDirection = MarData.ToVector2(_curMoveInputDistortion.LeftPath); break;
                    case Vector2 v when v.Equals(Vector2.right): moveDirection = MarData.ToVector2(_curMoveInputDistortion.RightPath); break;
                }
            }
        }

        //Switches hover to button in direction.
        private void Move_performed(InputAction.CallbackContext obj)
        {
            if (curHover == null) return;

            LinkMoveDirection();

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
            if (curHover == null) return;

            if (curHover.IsSelected) //Deselect if selected.
            {
                RemoveSelected(curHover);
                return; //Deselected piece.
            }

            if (curSelected.Count < _maxNumSelected) //Select if there is room.
            {
                AddSelected(curHover);

                if (curSelected.Count == _maxNumSelected && _confirmOnSelect)
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
                curSelected[i].LastPlayerID = _possessedPlayerID;
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
        {
            if (bo == null) return false;
            if (!bo.Hoverable) return false;

            return true;
            //(bo != null && bo.Interactable);
        }

        public bool HasCurHover()
        {
            if (!ReceiveInput) return false;
            if (curHover == null) return false;

            return true;
        }
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

    [System.Serializable]
    public class DistortedMoveInput
    {
        [SerializeField, Tooltip("The direction the up input will lead to instead of up.")]
        private EDirection2D _upPath = EDirection2D.UP;
        [SerializeField, Tooltip("The direction the up input will lead to instead of up.")]
        private EDirection2D _downPath = EDirection2D.DOWN;
        [SerializeField, Tooltip("The direction the up input will lead to instead of up.")]
        private EDirection2D _leftPath = EDirection2D.LEFT;
        [SerializeField, Tooltip("The direction the up input will lead to instead of up.")]
        private EDirection2D _rightPath = EDirection2D.RIGHT;

        #region GS
        public EDirection2D UpPath { get => _upPath; set => _upPath = value; }
        public EDirection2D DownPath { get => _downPath; set => _downPath = value; }
        public EDirection2D LeftPath { get => _leftPath; set => _leftPath = value; }
        public EDirection2D RightPath { get => _rightPath; set => _rightPath = value; }
        #endregion
    }
}

