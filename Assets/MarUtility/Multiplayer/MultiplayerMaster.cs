using MarUtility.ExecutionManagement;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility.Multiplayer
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class MultiplayerMaster : Manager
    {
        private static MultiplayerMaster inst;

        private PlayerInputManager piManager;

        #region GS
        public static MultiplayerMaster INST { get => inst; }
        public PlayerInputManager PiManager { get => piManager; }
        #endregion

        public override void Initialize()
        {
            if (inst == null) inst = this;
            else DebugMessages.MultipleMasterInstances("Multiplayer");

            piManager = GetComponent<PlayerInputManager>();

            base.Initialize();
        }
    }
}


