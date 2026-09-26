/*
 * Marlow Greenan
 * Created: 09/01/2026
 * Last Updated: 09/15/2026
 * 
 * Put in a persistant scene.
 */

using MarUtility.ExecutionManagement;
using NaughtyAttributes;
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
        private List<PlayerLinker> players = new List<PlayerLinker>();

        #region GS
        public static MultiplayerMaster INST { get => inst; }
        public PlayerInputManager PiManager { get => piManager; }
        public List<PlayerLinker> Players { get => players; }
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
            if (players.Count < MultiplayerManager.INST.MaxPlayerCount) //Under max player count.
                piManager.EnableJoining();
            else
            {
                while (Players.Count > MultiplayerManager.INST.MaxPlayerCount)
                {
                    Destroy(Players[Players.Count - 1]);
                    Players.RemoveAt(Players.Count - 1);
                }
                piManager.DisableJoining();
            }
        }

        public void SpawnAllPlayers()
        {
            for (int i = Players.Count - 1; i < MultiplayerManager.INST.MaxPlayerCount; i++)
                PiManager.JoinPlayer();
        }
    }
}


