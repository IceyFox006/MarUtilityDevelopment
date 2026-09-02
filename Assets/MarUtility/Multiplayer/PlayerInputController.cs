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
using UnityEngine.InputSystem;

namespace MarUtility.Multiplayer
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private PlayerInput playerInput;
        //private bool isLinked = false;

        [SerializeField, Label("Object Event System")]
        private ObjectEventSystem _oes;

        #region GS
        //public bool IsLinked { get => isLinked; set => isLinked = value; }
        #endregion

        public void Link(PlayerInput pi)
        {
            _oes.PlayerInput = pi;
            if (_oes != null) _oes.Initialize();

            playerInput = pi;
        }

        public bool IsLinked()
            => playerInput != null;
    }
}

