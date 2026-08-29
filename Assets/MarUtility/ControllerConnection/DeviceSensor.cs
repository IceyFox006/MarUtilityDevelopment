/*
 * Marlow Greenan
 * Created: 8/14/2026
 * Last Updated: 8/15/2026
 * 
 * Detects when buttons are pressed on the player inputs device.
 */
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility.DeviceManagement
{
    [RequireComponent(typeof(PlayerInput))]
    public class DeviceSensor : MonoBehaviour
    {
        private DeviceLinker linker;
        private PlayerInput pi;

        [SerializeField]
            private string _connectActionPath = "CONNECT";
        private InputAction connect;

        #region GS
        public DeviceLinker Linker { get => linker; set => linker = value; }
        #endregion

        private void OnDestroy()
        {
            DisableInput();    
        }

        public void Initialize(DeviceLinker l)
        {
            linker = l;
            pi = GetComponent<PlayerInput>();

            InitializeInput();
            EnableInput();
        }

        private void InitializeInput()
        {
            connect = pi.actions.FindAction(_connectActionPath);
        }

        private void EnableInput()
        {
            connect.performed += Connect_performed;
        }

        private void DisableInput()
        {
            connect.performed -= Connect_performed;
        }

        private void Connect_performed(InputAction.CallbackContext obj)
        {
            try
            {
                linker.LinkDevice(pi.devices[0]);
            }
            catch (ArgumentOutOfRangeException)
            {

            }

        }
    }
}

