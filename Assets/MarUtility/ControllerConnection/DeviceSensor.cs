using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility.DeviceManagement
{
    [RequireComponent(typeof(PlayerInput))]
    public class DeviceSensor : MonoBehaviour
    {
        private PlayerInput pi;

        [SerializeField]
            private string _connectActionPath = "CONNECT";
        private InputAction connect;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
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
                Debug.Log("Pressed " + pi.devices[0].ToString());
            }
            catch (ArgumentOutOfRangeException)
            {

            }

        }
    }
}

