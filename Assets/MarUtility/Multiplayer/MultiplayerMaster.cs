/*
 * Marlow Greenan
 * Created: 09/01/2026
 * Last Updated: 09/02/2026
 * 
 * Put in a persistant scene.
 */

using MarUtility.ExecutionManagement;
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


