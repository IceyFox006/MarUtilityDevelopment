using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility.Multiplayer
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class MultiplayerManager : Manager
    {
        private static MultiplayerManager inst;

        [SerializeField, OnValueChanged("OnVC_PiControllers"), Label("Controllers")]
        private List<PlayerInputController> _piControllers;

        private PlayerInputManager piManager;

        #region GS
        public static MultiplayerManager INST { get => inst; }
        public List<PlayerInputController> PiControllers { get => _piControllers; }
        #endregion

        public override void Initialize()
        {
            if (inst == null) inst = this;
            else DebugMessages.MultipleScriptInstances("Multiplayer Manager");

            piManager = GetComponent<PlayerInputManager>();

            base.Initialize();
        }

        public PlayerInputController GetFirstUnlinkedController()
        {
            for (int i = 0; i < _piControllers.Count; i++)
                if (!_piControllers[i].IsLinked) return _piControllers[i];

            return null;
        }

        #region Inspector
        private void OnVC_PiControllers()
        {
            piManager = GetComponent<PlayerInputManager>();

            if (_piControllers.Count <= piManager.maxPlayerCount) return;
            _piControllers.RemoveAt(_piControllers.Count - 1);
        }
        #endregion
    }
}

