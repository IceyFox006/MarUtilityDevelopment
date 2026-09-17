using MarUtility.ExecutionManagement;
using MarUtility.Multiplayer;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility
{
    public class MultiplayerDeviceSwitcher : Manager
    {
        private static MultiplayerDeviceSwitcher inst;

        [SerializeField]
        private bool _playersCanUseSameDevice = false;

        [SerializeField]
        private GameObject _deviceSensor;

        [SerializeField, BoxGroup("Control Scheme IDs"), Label("Control Scheme Gamepad ID")]
        private string _csGamepadID = "Gamepad";
        [SerializeField, BoxGroup("Control Scheme IDs"), Label("Control Scheme Keyboard ID")]
        private string _csKeyboardID = "Keyboard";
        [SerializeField, BoxGroup("Control Scheme IDs"), Label("Control Scheme Player 0 Keyboard ID")]
        private string _csP0KeyboardID = "Keyboard_Player0";
        [SerializeField, BoxGroup("Control Scheme IDs"), Label("Control Scheme Player 1 Keyboard ID")]
        private string _csP1KeyboardID = "Keyboard_Player1";

        [SerializeField, BoxGroup("Components")]
        private CanvasGroup _switchDeviceUICG;

        [SerializeField, ReadOnly]
        private int playerSwitching = 0;

        #region GS
        public static MultiplayerDeviceSwitcher INST { get => inst; }
        public int PlayerSwitching { get => playerSwitching; }
        #endregion

        public override void Initialize()
        {
            if (inst == null) inst = this;
            else DebugMessages.MultipleScriptInstances("MultiplayerDeviceSwitcher");

            base.Initialize();
        }

        public void BeginDeviceSwitchSequence()
        {
            if (!_playersCanUseSameDevice && DeviceMaster.INST.ValidDevices.Count < MultiplayerManager.INST.MaxPlayerCount)
                return;

            playerSwitching = 0;

            //Disable all input.
            MultiplayerManager.INST.DisableAllPlayerInput();

            //Show switch device UI.
            _switchDeviceUICG.alpha = 1;

            //Spawn device sensors.
            SpawnSensors();
        }

       

        public void EndSwitchDeviceSequence()
        {
            //Destroy all device sensors.

            //Hide switch device UI.
            _switchDeviceUICG.alpha = 0;
        }

        //Spawns device sensors and assigns each of them a device.
        private void SpawnSensors()
        {
            DeviceSensor curDS;
            foreach (InputDevice d in DeviceMaster.INST.ValidDevices)
            {
                curDS = Instantiate(_deviceSensor, transform).GetComponent<DeviceSensor>();
                curDS.Initialize(d.name);

                curDS.Pi.SwitchCurrentControlScheme(d);
            }
        }

        //INITIATE SEQUENCE
        //Disable all input.
        //Show swap device UI.
        //Spawn device sensors.
        //
    }
}

