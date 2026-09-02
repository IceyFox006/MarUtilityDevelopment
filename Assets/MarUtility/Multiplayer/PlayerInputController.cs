using MarUtility.UIExtensions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility.Multiplayer
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private bool isLinked = false;

        [SerializeField, Label("Object Event System")]
        private ObjectEventSystem _oes;

        #region GS
        public bool IsLinked { get => isLinked; set => isLinked = value; }
        #endregion

        public void Link(PlayerInput pi)
        {
            _oes.PlayerInput = pi;
            if (_oes != null) _oes.Initialize();

            isLinked = true;
        }
    }
}

