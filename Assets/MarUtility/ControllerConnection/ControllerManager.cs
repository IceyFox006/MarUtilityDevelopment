/*
 * Marlow Greenan
 * Created: 8/12/2026
 * Last Updated: 8/12/2026
 * 
 * Manages controller connection to the game.
 */
using MarUtility.ExecutionManagement;
using MarUtility.ObjectEventSystem;
using NaughtyAttributes;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ControllerManager : Manager
{
    [SerializeField]
        private bool _checkOnIntitialize = true;
    private bool isChecking;

    [SerializeField, Tooltip("If controller checks occur on every SceneLoader tick.")]
        private bool _checkIntervalIsTick;
    [SerializeField, HideIf("_checkIntervalIsTick")]
        private float _checkInterval = 1f;

    [SerializeField]
        private UnityEvent _onControllerConnection;
    [SerializeField]
        private UnityEvent _onControllerDisconnection;

    [SerializeField]
        private PlayerInput[] _playerInputs;
    private InputDevice[] inputDevices;
    private List<ConnectionData> connections = new List<ConnectionData>();

    private int prevDeviceCount = 0;

    public override void Initialize()
    {
        CreateConnections();

        if (_checkOnIntitialize)
            StartCoroutine(ControllerCheckInterval());

        base.Initialize();
    }

    //Creates the list of connections and assigns player inputs to each one.
    private void CreateConnections()
    {
        for (int i = 0; i < _playerInputs.Length; i++)
            connections.Add(new ConnectionData(_playerInputs[i]));
    }

    //Checks for changes in controllers every interval.
    private IEnumerator ControllerCheckInterval()
    {
        isChecking = true;
        while (isChecking)
        {
            yield return new WaitForSeconds((_checkIntervalIsTick)? SceneLoader.INST.TickInterval : _checkInterval);

            inputDevices = InputSystem.devices.ToArray();

            CheckDeviceCount();

            prevDeviceCount = inputDevices.Length;
        }
    }

    //Checks if a device has been added or removed.
    private void CheckDeviceCount()
    {
        if (inputDevices.Length > prevDeviceCount) //New device added.
            ConnectDevice(inputDevices.Length - 1);
        else if (inputDevices.Length < prevDeviceCount) //Device removed.
            DisconnectDevice();
    }

    private void ConnectDevice(int deviceIndex)
    {
        if (FindDeviceInConnection(inputDevices[deviceIndex]) != -1) return; //Device is already connected.

        for (int i = 0; i < connections.Count; i++)
        {
            if (connections[i].IsConnected()) continue; //Connection already has device.

            connections[i].Device = inputDevices[deviceIndex];
            Debug.Log("Connected " + inputDevices[inputDevices.Length - 1].ToString());
        }

    }

    private void DisconnectDevice()
    {
        Debug.Log("Disconnected");
    }

    #region Data
    private int FindDeviceInConnection(InputDevice device)
        => FindDeviceInConnection(device.ToString());
    private int FindDeviceInConnection(string deviceName)
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
    private class ConnectionData
    {
        private PlayerInput playerInput;
        private InputDevice device;
        public ConnectionData(PlayerInput pi)
        {
            playerInput = pi;
        }

        public InputDevice Device { get => device; set => device = value; }

        public bool IsConnected()
            => device != null;
    }
    //public override void Initialize()
    //{
    //    CreateDeviceList();

    //    if (_checkOnIntitialize)
    //        StartCoroutine(ControllerCheckInterval());

    //    base.Initialize();
    //}

    //private void CreateDeviceList()
    //{
    //    foreach (PlayerInput pi in _playerInputs)
    //        devices.Add(new Device(pi));
    //}

    //private IEnumerator ControllerCheckInterval()
    //{
    //    isChecking = true;
    //    while (isChecking)
    //    {
    //        yield return new WaitForSeconds((_checkIntervalIsTick) ? SceneLoader.INST.TickInterval : _checkInterval);

    //        CheckControllers();
    //    }
    //}

    //public void CheckControllers()
    //{
    //    inputDevices = InputSystem.devices.ToArray();

    //    if (inputDevices.Count() > prevControllerCount) //Connected controller.
    //        ConnectDevice(inputDevices.Length - 1);
    //    else if (inputDevices.Count() < prevControllerCount) //Disconnected controller.
    //        DisconnectController();

    //    prevControllerCount = inputDevices.Length;
    //}

    //private void ConnectDevice(int deviceIndex)
    //{
    //    for (int i = 0; i < devices.Count; i++)
    //    {
    //        if (devices[i].IsPossessed()) continue;
    //        if (IsConnected(inputDevices[deviceIndex])) continue;

    //        Debug.Log("Connected " + inputDevices[deviceIndex].ToString());
    //        devices[i].IDevice = inputDevices[deviceIndex];
    //        _onControllerConnection.Invoke();
    //    }

    //}

    //private void DisconnectController()
    //{
    //    Debug.Log("Disconnected.");
    //    _onControllerDisconnection.Invoke();
    //}

    //private int FindDevice(InputDevice iDevice)
    //    => FindDevice(iDevice.ToString());
    //private int FindDevice(string iDeviceName)
    //{
    //    for (int i = 0; i < devices.Count; i++)
    //    {
    //        if (devices[i].IDevice.ToString().Equals(iDeviceName))
    //            return i;
    //    }
    //    return -1;
    //}

    //#region Bool Check
    //private bool IsConnected(InputDevice iDevice)
    //    => IsConnected(iDevice.ToString());

    //private bool IsConnected(string iDeviceName)
    //{
    //    int index = FindDevice(iDeviceName);
        
    //    if (index < 0) return false;
    //    if (!devices[index].IsPossessed()) return false;

    //    return true;
    //}
    //#endregion
    ////=================================================================================================================
    //private class Device
    //{
    //    private PlayerInput playerInput;
    //    private InputDevice iDevice;

    //    public Device(PlayerInput pi)
    //    {
    //        playerInput = pi;
    //    }

    //    public InputDevice IDevice 
    //    { 
    //        get => iDevice; 
    //        set //=> iDevice = value; 
    //        {
    //            iDevice = value;
    //            playerInput.SwitchCurrentControlScheme(iDevice);
    //        }
    //    }

    //    public bool IsPossessed()
    //        => iDevice != null;
    //}
}
