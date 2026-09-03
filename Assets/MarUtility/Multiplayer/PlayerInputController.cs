/*
 * Marlow Greenan
 * Created: 09/01/2026
 * Last Update: 09/02/2026
 * 
 * Holds components that will need to be linked to a player input.
 */

using MarUtility.UIExtensions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MarUtility.Multiplayer
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private PlayerInput playerInput;

        [SerializeField, Label("Object Event System")]
        private ObjectEventSystem _oes;

        [SerializeField]
        private UnityEvent _onLink;

        #region GS
        public PlayerInput PlayerInput { get => playerInput; }
        #endregion

        public void Link(PlayerInput pi)
        {
            _oes.PlayerInput = pi;
            if (_oes != null) _oes.Initialize();

            playerInput = pi;
            _onLink.Invoke();
        }

        public bool IsLinked()
            => playerInput != null;
    }
}

