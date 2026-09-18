using MarUtility.ExecutionManagement;
using MarUtility.Multiplayer;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility
{
    public class MultiplayerDeviceSwitcher : Manager
    {
        private static MultiplayerDeviceSwitcher inst;

        [SerializeField]
        private bool _playersCanUseSameKeyboard = true;

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
        [SerializeField, ReadOnly]
        private List<DeviceSensor> sensors = new List<DeviceSensor>();
        [SerializeField, ReadOnly]
        private Dictionary<DeviceSensor, int> connectionQueue = new Dictionary<DeviceSensor, int>();

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

        //Begins the device switch sequence.
        public void BeginDeviceSwitchSequence()
        {
            //if (!_playersCanUseSameDevice && DeviceMaster.INST.ValidDevices.Count < MultiplayerManager.INST.MaxPlayerCount)
            //    return;

            playerSwitching = 0;

            //Disable all input.
            MultiplayerManager.INST.DisableAllPlayerInput();

            //Show switch device UI.
            _switchDeviceUICG.alpha = 1;

            //Spawn device sensors.
            SpawnSensors();
        }

        //Connects the device with device name to the next player.
       public void AddDeviceToConnectionQueue(DeviceSensor sensor)
       {
            //InputDevice d = DeviceMaster.INST.FindDevice(sensor.DeviceID);

            //if (d != null)
            //{
            //    MultiplayerMaster.INST.Players[playerSwitching].Pi.SwitchCurrentControlScheme(d);
            //    DestroySensor(sensor);
            //}

            connectionQueue.Add(sensor, playerSwitching);

            playerSwitching++;

            if (playerSwitching >= MultiplayerMaster.INST.Players.Count)
                EndSwitchDeviceSequence();
       }

        private void ConnectDevicesToPlayers()
        {
            InputDevice d;
            foreach (KeyValuePair<DeviceSensor, int> kvp in connectionQueue)
            {
                d = DeviceMaster.INST.FindDevice(kvp.Key.DeviceID);

                if (d != null)
                {
                    MultiplayerMaster.INST.Players[kvp.Value].Pi.SwitchCurrentControlScheme(d);
                    DestroySensor(kvp.Key);
                }
            }
            connectionQueue.Clear();
        }

        public void EndSwitchDeviceSequence()
        {
            ConnectDevicesToPlayers();

            //Destroy all device sensors.
            DestroySensors();

            //Hide switch device UI.
            _switchDeviceUICG.alpha = 0;

            //Enable player input.
            MultiplayerManager.INST.EnableAllPlayerInput();
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

                sensors.Add(curDS);
            }
        }

        private void DestroySensors()
        {
            for (int i = sensors.Count - 1; i >= 0; i--)
                DestroySensor(i);
        }

        private void DestroySensor(DeviceSensor sensor)
        {
            int i = FindSensor(sensor);
            if (i == -1)
                return;
            DestroySensor(i);
        }

        private void DestroySensor(int i)
        {
            Destroy(sensors[i].gameObject);
            sensors.RemoveAt(i);
        }

        private int FindSensor(DeviceSensor sensor)
        {
            for (int i = 0; i < sensors.Count; i++)
            {
                if (sensors[i].DeviceID.Equals(sensor.DeviceID))
                    return i;
            }
            return -1;
        }

       

        //INITIATE SEQUENCE
        //Disable all input.
        //Show swap device UI.
        //Spawn device sensors.
        //
    }
}

