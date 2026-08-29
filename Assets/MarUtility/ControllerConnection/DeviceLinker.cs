/*
 * Marlow Greenan
 * Created: 8/14/2026
 * Last Updated: 8/15/2026
 * 
 * Links devices to players after a button has been pressed on the desired controller.
 */
using MarUtility.DeviceManagement;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceLinker : MonoBehaviour
{
    [SerializeField]
        private GameObject _connectDeviceScreen;

    [SerializeField]
        private GameObject _deviceSensorPrefab;
    private List<DeviceSensor> sensors = new List<DeviceSensor>();

    [SerializeField, ReadOnly]
    private bool inLinkingProcess = false;
    [SerializeField, ReadOnly]
    private int playerLinking = 0;


    //Begins sequence of linking devices to players.
    public void BeginLink()
    {
        if (inLinkingProcess) return;

        inLinkingProcess = true;
        sensors = new List<DeviceSensor>();

        DeviceManager.INST.DisablePlayerInputs();

        foreach (InputDevice device in DeviceManager.INST.InputDevices)
        {
            DeviceSensor ds = Instantiate(_deviceSensorPrefab, transform).GetComponent<DeviceSensor>();
            ds.Initialize(this);
            sensors.Add(ds);
        }

        playerLinking = 0;
    }

    public void LinkDevice(InputDevice device)
    {
        Debug.Log("Connected " + device.ToString() + " to " + DeviceManager.INST.PiControllers[playerLinking].Player.Name);
        DeviceManager.INST.PiControllers[playerLinking].Player.Device = device;
        //DeviceManager.INST.Connections[playerLinking].Device = device;
        playerLinking++;

        if (playerLinking >= DeviceManager.INST.PiControllers.Length) //Linked all players.
            EndLink();
    }

    //Ends the sequence of linking devices to players.
    public void EndLink()
    {
        if (!inLinkingProcess) return;

        inLinkingProcess = false;

        DeviceManager.INST.EnablePlayerInputs();

        _connectDeviceScreen.GetComponent<CanvasGroup>().alpha = 0f;
        for (int i = sensors.Count - 1; i >= 0; i--)
            Destroy(sensors[i].gameObject);


        for (int i = 0; i < DeviceManager.INST.PiControllers.Length; i++)
        {
            DeviceManager.INST.Connections[i].Device = DeviceManager.INST.PiControllers[i].Player.Device;
        }

    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < DeviceManager.INST.PiControllers.Length; i++)
        {
            DeviceManager.INST.Connections[i].Device = DeviceManager.INST.PiControllers[i].Player.Device;
        }
    }
}
