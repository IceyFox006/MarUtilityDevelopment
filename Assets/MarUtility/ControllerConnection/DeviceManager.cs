/*
 * Marlow Greenan
 * Created: 8/12/2026
 * Last Updated: 8/15/2026
 * 
 * Manages device (controller) connection to the game.
 */
using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace MarUtility.DeviceManagement
{
    public class DeviceManager : Manager
    {
        public static DeviceManager INST;

        [SerializeField]
        private bool _checkOnIntitialize = true;
        private bool isChecking;

        [SerializeField, Tooltip("If controller checks occur on every SceneLoader tick.")]
        private bool _checkIntervalIsTick;
        [SerializeField, HideIf("_checkIntervalIsTick")]
        private float _checkInterval = 1f;

        [SerializeField]
        private bool _autoConnectToPlayer = true;
        [SerializeField]
        private UnityEvent _onControllerConnection;
        [SerializeField]
        private UnityEvent _onControllerDisconnection;

        [SerializeField]
        private PlayerInputController[] _piControllers;
        private InputDevice[] inputDevices;
        private List<ConnectionData> connections = new List<ConnectionData>();

        private int prevDeviceCount = 0;

        #region GS
        public PlayerInputController[] PiControllers { get => _piControllers; }
        public InputDevice[] InputDevices { get => inputDevices; }
        public List<ConnectionData> Connections { get => connections; }
        #endregion

        public override void Initialize()
        {
            if (INST == null) INST = this;
            else DebugMessages.MultipleScriptInstances("DeviceManager");

            foreach (PlayerInputController pic in _piControllers)
                pic.Initialize();

            CreateConnections();

            if (_checkOnIntitialize)
                StartCoroutine(ControllerCheckInterval());

            base.Initialize();
        }

        //Creates the list of connections and assigns player inputs to each one.
        private void CreateConnections()
        {
            for (int i = 0; i < _piControllers.Length; i++)
                connections.Add(new ConnectionData(_piControllers[i]));
        }

        //Checks for changes in controllers every interval.
        private IEnumerator ControllerCheckInterval()
        {
            isChecking = true;
            while (isChecking)
            {
                yield return new WaitForSeconds((_checkIntervalIsTick) ? SceneLoader.INST.TickInterval : _checkInterval);

                inputDevices = InputSystem.devices.ToArray();

                CheckDeviceCount();

                prevDeviceCount = inputDevices.Length;
            }
        }

        //Checks if a device has been added or removed.
        private void CheckDeviceCount()
        {
            if (inputDevices.Length > prevDeviceCount) //New device added.
            {
                _onControllerConnection.Invoke();

                if (_autoConnectToPlayer)
                    ConnectDevice(inputDevices.Length - 1);
            }
            else if (inputDevices.Length < prevDeviceCount) //Device removed.
                DisconnectDevice();
        }

        //private void ConnectDevice(int deviceIndex)
        //{
        //    if (FindDeviceInConnections(inputDevices[deviceIndex]) != -1)
        //    {
        //        Debug.Log(inputDevices[deviceIndex].ToString() + " is already connected.");
        //        return; //Device is already connected.
        //    }
        //    InputUser.PerformPairingWithDevice(inputDevices[deviceIndex]);
        //}

        //Connects a device to a player input that does not have a device.
        private void ConnectDevice(int deviceIndex)
        {
            if (FindDeviceInConnections(inputDevices[deviceIndex]) != -1)
            {
                Debug.Log(inputDevices[deviceIndex].ToString() + " is already connected.");
                return; //Device is already connected.
            }

            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].IsConnected()) continue; //Connection already has device.

                //if (InputUser.all.Count < 2)
                //{
                //    InputUser.PerformPairingWithDevice(inputDevices[deviceIndex]);
                //}
                //else
                    connections[i].Device = inputDevices[deviceIndex];
                Debug.Log("Connected " + inputDevices[inputDevices.Length - 1].ToString());

                break;
            }
        }

        private void DisconnectDevice()
        {
            _onControllerDisconnection.Invoke();
            Debug.Log("Disconnected");
        }

        public void EnablePlayerInputs()
        {
            foreach (PlayerInputController pi in _piControllers)
                pi.EnableAllInput();
        }
        public void DisablePlayerInputs()
        {
            foreach (PlayerInputController pi in _piControllers)
                pi.DisableAllInput();
        }
        #region Data
        //Finds a device in the list of connections and returns -1 if the device is not part of a connection.
        private int FindDeviceInConnections(InputDevice device)
            => FindDeviceInConnections(device.ToString());
        private int FindDeviceInConnections(string deviceName)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].Device == null) continue;
                if (connections[i].Device.ToString().Equals(deviceName)) return i;
            }

            return -1;
        }
        #endregion
        //=================================================================================================================
        public class ConnectionData
        {
            private PlayerInputController controller;
            private InputDevice device;
            public ConnectionData(PlayerInputController pic)
            {
                controller = pic;
            }

            public InputDevice Device
            {
                get => device;
                set// => device = value;
                {
                    device = value;
                    controller.Input.SwitchCurrentControlScheme(device);
                }
            }

            public bool IsConnected()
                => device != null;
        }
    }
}

