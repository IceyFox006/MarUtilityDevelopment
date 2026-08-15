using MarUtility;
using MarUtility.DeviceManagement;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.InputSystem;

public class DeviceLinker : MonoBehaviour
{
    [SerializeField]
        private GameObject _deviceSensorPrefab;
    private List<DeviceSensor> sensors = new List<DeviceSensor>();

    [SerializeField, ReadOnly]
    private bool inLinkingProcess = false;
    [SerializeField, ReadOnly]
    private int playerLinking = 0;

    public void BeginLink()
    {
        if (inLinkingProcess) return;

        inLinkingProcess = true;
        foreach (InputDevice device in DeviceManager.INST.InputDevices)
        {
            GameObject go = Instantiate(_deviceSensorPrefab, transform);
            sensors.Add(go.GetComponent<DeviceSensor>());
        }

        playerLinking = 0;
    }

    public void EndLink()
    {
        if (!inLinkingProcess) return;

        inLinkingProcess = false;
        EventMethod.DestroyChildren(transform);
    }
}
