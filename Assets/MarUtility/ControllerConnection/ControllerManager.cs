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
        private List<ObjectEventSystem> systems;
    private InputDevice[] devices;
    private int prevControllerCount = 0;

    public override void Initialize()
    {
        if (_checkOnIntitialize)
            StartCoroutine(ControllerCheckInterval());

        base.Initialize();
    }

    private IEnumerator ControllerCheckInterval()
    {
        isChecking = true;
        while (isChecking)
        {
            yield return new WaitForSeconds((_checkIntervalIsTick) ? SceneLoader.INST.TickInterval : _checkInterval);

            CheckControllers();
        }
    }

    public void CheckControllers()
    {
        devices = InputSystem.devices.ToArray();

        if (devices.Count() > prevControllerCount) //Connected controller.
            ConnectController();
        else if (devices.Count() < prevControllerCount) //Disconnected controller.
            DisconnectController();

        prevControllerCount = devices.Length;
    }

    private void ConnectController()
    {
        Debug.Log("Connected " + devices[devices.Length - 1].ToString());
        _onControllerConnection.Invoke();
    }

    private void DisconnectController()
    {
        Debug.Log("Disconnected.");
        _onControllerDisconnection.Invoke();
    }
}
