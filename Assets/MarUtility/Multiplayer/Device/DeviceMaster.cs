/*
 * Marlow Greenan
 * Created: 09/16/2026
 * Last Updated: 09/18/2026 by Marlow Greenan
 * 
 * Detects when a device is connected or disconnected.
 */
using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MarUtility.Multiplayer
{
    public class DeviceMaster : Manager
    {
        private static DeviceMaster inst;

        [SerializeField, MinValue(0.1f), Tooltip("How often it checks for new devices.")]
        private float _checkInterval = 1f;

        //EVENTS
        [SerializeField, BoxGroup("Event")]
        private UnityEvent _onDeviceConnected;
        [SerializeField, BoxGroup("Event")]
        private UnityEvent _onDeviceDisconnected;

        [SerializeField, ReadOnly]
        private int deviceCount = 0;
        private List<InputDevice> validDevices = new List<InputDevice>();

        #region GS
        public static DeviceMaster INST { get => inst; }
        public List<InputDevice> ValidDevices { get => validDevices; }
        #endregion

        public override void Initialize()
        {
            if (inst == null) inst = this;
            else DebugMessages.MultipleMasterInstances("Device");

            StartCoroutine(CheckDevicesInterval());

            base.Initialize();
        }

        //Checks if a device is connected or disconnected.
        private IEnumerator CheckDevicesInterval()
        {
            while (true)
            {
                yield return new WaitForSeconds(_checkInterval);
                
                deviceCount = validDevices.Count;
                LinkValidDevices();

                if (validDevices.Count > deviceCount)
                    DeviceConnected();
                else if (validDevices.Count < deviceCount)
                    DeviceDisconnected();
            }
        }

        //Add keyboards and gamepads to valid devices.
        private void LinkValidDevices()
        {
            validDevices.Clear();

            InputDevice[] devices = InputSystem.devices.ToArray();

            foreach (InputDevice d in devices)
            {
                switch (d)
                {
                    case Keyboard:
                    case Gamepad:
                        validDevices.Add(d); break;
                }
            }
        }
    
        //Finds a device with the matching name.
        public InputDevice FindDevice(string deviceName)
        {
            for (int i = 0; i < validDevices.Count; i++)
            {
                if (validDevices[i].name.Equals(deviceName))
                    return validDevices[i];
            }

            return null;
        }

        private void DeviceConnected()
        {
            _onDeviceConnected.Invoke();
        }

        private void DeviceDisconnected()
        {
            _onDeviceDisconnected.Invoke();
        }
    }
}

