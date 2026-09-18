/*
 * Marlow Greenan
 * Created: 09/17/2026
 * Last Updated: 09/18/2026 by Marlow Greenan
 * 
 * Allows the switching of devices between players.
 */
using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility.Multiplayer
{
    public class MultiplayerDeviceSwitcher : Manager
    {
        private static MultiplayerDeviceSwitcher inst;

        [SerializeField]
        private bool _playersCanUseSameKeyboard = true;
        [SerializeField]
        private bool _allowNewDeviceMidSwitch = true;

        [SerializeField]
        private GameObject _deviceSensor;

        //CONTROL SCHEME
        [SerializeField, BoxGroup("Control Scheme IDs"), Label("Control Scheme Gamepad ID")]
        private string _csGamepadID = "Gamepad";
        [SerializeField, BoxGroup("Control Scheme IDs"), Label("Control Scheme Keyboard ID")]
        private string _csKeyboardID = "Keyboard";
        [SerializeField, BoxGroup("Control Scheme IDs"), Label("Control Scheme Player 0 Keyboard ID"), ShowIf("_playersCanUseSameKeyboard")]
        private string _csP0KeyboardID = "Keyboard_Player0";
        [SerializeField, BoxGroup("Control Scheme IDs"), Label("Control Scheme Player 1 Keyboard ID"), ShowIf("_playersCanUseSameKeyboard")]
        private string _csP1KeyboardID = "Keyboard_Player1";

        //UI COMPONENTS
        [SerializeField, BoxGroup("Components, UI")]
        private CanvasGroup _switchDeviceUICG;
        [SerializeField, BoxGroup("Components UI")]
        private TMP_Text _playerSwitchingText;

        //READ ONLY
        [SerializeField, ReadOnly]
        private bool isSwitching = false;
        [SerializeField, ReadOnly]
        private int playerSwitching = 0;
        [SerializeField, ReadOnly]
        private List<DeviceSensor> sensors = new List<DeviceSensor>();
        [SerializeField, ReadOnly]
        private Dictionary<int, DeviceSensor> connectionQueue = new Dictionary<int, DeviceSensor>();

        #region GS
        public static MultiplayerDeviceSwitcher INST { get => inst; }
        public int PlayerSwitching { get => playerSwitching; }
        #endregion

        public override void Initialize()
        {
            if (inst == null) inst = this;
            else DebugMessages.MultipleScriptInstances("MultiplayerDeviceSwitcher");

            //Add events to on device connected & disconnected.
            DeviceMaster.INST.OnDeviceConnected.AddListener( delegate { BeginDeviceSwitchSequence(); });
            DeviceMaster.INST.OnDeviceDisconnected.AddListener( delegate { BeginDeviceSwitchSequence(); });

            if (_allowNewDeviceMidSwitch)
                DeviceMaster.INST.OnDeviceConnected.AddListener( delegate { SpawnSensorsForNewDevices(); });

            base.Initialize();
        }

        //Begins the device switch sequence.
        public void BeginDeviceSwitchSequence()
        {
            if (isSwitching) return;
            if (!_playersCanUseSameKeyboard && DeviceMaster.INST.ValidDevices.Count < MultiplayerManager.INST.MaxPlayerCount) return;

            isSwitching = true;

            playerSwitching = 0;

            //Disable all input.
            MultiplayerManager.INST.DisableAllPlayerInput();

            //Show switch device UI.
            _switchDeviceUICG.alpha = 1;
            _playerSwitchingText.text = "Player " + playerSwitching + " press a button on an unlinked controller.";

            //Spawn device sensors.
            SpawnSensors();
        }

        //Connects the device with device name to the next player.
       public void AddDeviceToConnectionQueue(DeviceSensor sensor)
       {
            InputDevice d = DeviceMaster.INST.FindDevice(sensor.DeviceID);
            bool connectAndDestroySensor = false;

            if (d != null)
            {
                switch (d)
                {
                    case Keyboard:
                        if (_playersCanUseSameKeyboard) //2 players can be on the same keybaord.
                            connectionQueue.Add(playerSwitching, sensor);
                        else connectAndDestroySensor = true;
                        break;
                    case Gamepad: //2 players cannot uses the same gamepad. Link then destroy the sensor.
                        connectAndDestroySensor = true; break;
                }
            }

            if (connectAndDestroySensor)
            {
                MultiplayerMaster.INST.Players[playerSwitching].Pi.SwitchCurrentControlScheme(d);
                DestroySensor(sensor);
            }

            playerSwitching++;

            _playerSwitchingText.text = "Player " + playerSwitching + " press a button on an unlinked controller.";

            if (playerSwitching >= MultiplayerMaster.INST.Players.Count)
                EndSwitchDeviceSequence();
       }

        //Iterates through the connection queue and connects players to devices.
        private void ConnectDevicesToPlayers()
        {
            InputDevice d;
            foreach (KeyValuePair<int, DeviceSensor> kvp in connectionQueue)
            {
                d = DeviceMaster.INST.FindDevice(kvp.Value.DeviceID);

                if (d != null)
                {
                    switch (d)
                    {
                        case Keyboard:
                            MultiplayerMaster.INST.Players[kvp.Key].Pi.SwitchCurrentControlScheme("Keyboard_Player" + kvp.Key, d); break;
                        default:
                            MultiplayerMaster.INST.Players[kvp.Key].Pi.SwitchCurrentControlScheme(d); break;
                    }
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

            isSwitching = false;
        }

        #region Sensors
        //Spawns device sensors and assigns each of them a device.
        private void SpawnSensors()
        {
            DeviceSensor curDS;
            foreach (InputDevice d in DeviceMaster.INST.ValidDevices)
                SpawnSensor(d);
        }

        //Spawns sensors for devices that do not already have sensors.
        private void SpawnSensorsForNewDevices()
        {
            InputDevice device = null;
            DeviceSensor sensor;
            bool foundMatch;
            for (int vd = 0; vd < DeviceMaster.INST.ValidDevices.Count; vd++)
            {
                foundMatch = false;
                for (int ds = 0; ds < sensors.Count; ds++)
                {
                    device = DeviceMaster.INST.ValidDevices[vd];
                    sensor = sensors[ds];

                    if (device.name.Equals(sensor.DeviceID)) //Device already has a sensor.
                    {
                        foundMatch = true;
                        break;
                    }
                }
                if (!foundMatch)
                    SpawnSensor(device);
            }
        }

        //Spawns a sensor for a device.
        private void SpawnSensor(InputDevice d)
        {
            if (d == null) return;

            DeviceSensor curDS = Instantiate(_deviceSensor, transform).GetComponent<DeviceSensor>();
            curDS.Initialize(d.name);
            curDS.Pi.SwitchCurrentControlScheme(d);

            sensors.Add(curDS);
        }

        //Destroys all censors.
        private void DestroySensors()
        {
            for (int i = sensors.Count - 1; i >= 0; i--)
                DestroySensor(i);
        }

        //Destroys the sensor and removes it from the sensors list.
        private void DestroySensor(DeviceSensor sensor)
        {
            int i = FindSensor(sensor);
            if (i == -1)
                return;
            DestroySensor(i);
        }

        //Destroys sensor at i and removes it from the sensors list.
        private void DestroySensor(int i)
        {
            if (i >= sensors.Count) return;

            Destroy(sensors[i].gameObject);
            sensors.RemoveAt(i);
        }

        //Returns the index of the sensor in sensors.
        private int FindSensor(DeviceSensor sensor)
        {
            for (int i = 0; i < sensors.Count; i++)
            {
                if (sensors[i].DeviceID.Equals(sensor.DeviceID))
                    return i;
            }
            return -1;
        }
        #endregion

    }
}

