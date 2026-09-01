using MarUtility.ExecutionManagement;
using MarUtility.UIExtensions;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.InputSystem;

namespace MarUtility.Multiplayer
{
    public class PlayerInputController : Manager
    {
        [SerializeField, ReadOnly]
        private bool isLinked = false;

        [SerializeField]
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

