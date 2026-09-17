using MarUtility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class DeviceSensor : MonoBehaviour, IInput
{
    [SerializeField, ReadOnly]
    private string deviceID;

    [SerializeField, BoxGroup("Action IDs")]
    private string _actionConnectID = "CONNECT";

    private PlayerInput pi;
    private InputAction connect;

    #region GS
    public PlayerInput Pi { get => pi; }
    public string DeviceID { get => deviceID; }
    #endregion

    public void Initialize(string dID)
    {
        pi = GetComponent<PlayerInput>();
        deviceID = dID;

        InitializeInput();
        EnableInput();
    }
    private void OnDestroy()
    {
        DisableInput();
    }

    public void InitializeInput()
    {
        connect = pi.actions.FindAction(_actionConnectID);
    }

    public void EnableInput()
    {
        connect.performed += Connect_performed;
    }

    public void DisableInput()
    {
        connect.performed -= Connect_performed;
    }

    private void Connect_performed(InputAction.CallbackContext obj)
    {
        Debug.Log(deviceID);
    }
}
