/*
 * Marlow Greenan
 * Created: 09/01/2026
 * Last Updated: 09/02/2026
 * 
 * Put in a persistant scene.
 */

using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility.Multiplayer
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class MultiplayerMaster : Manager
    {
        private static MultiplayerMaster inst;

        private PlayerInputManager piManager;

        [SerializeField, ReadOnly]
        private List<GameObject> playerInputs = new List<GameObject>();

        #region GS
        public static MultiplayerMaster INST { get => inst; }
        public PlayerInputManager PiManager { get => piManager; }
        public List<GameObject> PlayerInputs { get => playerInputs; }
        #endregion

        public override void Initialize()
        {
            if (inst == null) inst = this;
            else DebugMessages.MultipleMasterInstances("Multiplayer");

            piManager = GetComponent<PlayerInputManager>();

            base.Initialize();
        }

        public void UpdatePlayerCount()
        {
            if (playerInputs.Count < MultiplayerManager.INST.MaxPlayerCount) //Under max player count.
            {
                piManager.EnableJoining();
            }
            else
            {
                while (PlayerInputs.Count > MultiplayerManager.INST.MaxPlayerCount)
                {
                    Destroy(PlayerInputs[PlayerInputs.Count - 1]);
                    PlayerInputs.RemoveAt(PlayerInputs.Count - 1);
                }
                piManager.DisableJoining();
            }
        }
    }
}


