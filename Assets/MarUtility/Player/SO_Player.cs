using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptable Objects/MarUtility/Player")]
public class SO_Player : ScriptableObject
{
    [SerializeField]
        private string _playerName;
    [SerializeField, ReadOnly]
        private InputDevice device;

    #region GS
    public InputDevice Device { get => device; set => device = value; }
    public string Name { get => _playerName; set => _playerName = value; }
    #endregion
}
