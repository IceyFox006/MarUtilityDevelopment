using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility.Multiplayer
{
    public class MultiplayerManager : Manager
    {
        private static MultiplayerManager inst;

        //[SerializeField, OnValueChanged("OnVC_PiControllers"), Label("Controllers")]
        //private List<PlayerInputController> _piControllers;
        [SerializeField, Label("Controllers")]
        private Dictionary<string, PlayerInputController> _piControllers;

        #region GS
        public static MultiplayerManager INST { get => inst; }
        public Dictionary<string, PlayerInputController> PiControllers { get => _piControllers; set => _piControllers = value; }

        //public List<PlayerInputController> PiControllers { get => _piControllers; }
        #endregion

        public override void Initialize()
        {
            if (inst == null) inst = this;
            else DebugMessages.MultipleScriptInstances("Multiplayer Manager");

            base.Initialize();
        }

        public KeyValuePair<string, PlayerInputController> GetFirstUnlinkedController()
        {
            //for (int i = 0; i < _piControllers.Count; i++)
            //    if (!_piControllers[i].IsLinked) return _piControllers[i];
            foreach (KeyValuePair<string, PlayerInputController> pic in _piControllers)
                if (!pic.Value.IsLinked) return pic;

            return new KeyValuePair<string, PlayerInputController>("", null);
        }

        //#region Inspector
        //private void OnVC_PiControllers()
        //{
        //    PlayerInputManager piManager = FindAnyObjectByType<MultiplayerMaster>().GetComponent<PlayerInputManager>();
        //    if (_piControllers.Count <= piManager.maxPlayerCount) return;
        //    _piControllers.Remove(_piControllers.Keys.Last());
        //}
        //#endregion
    }
}

