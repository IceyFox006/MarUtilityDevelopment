/*
 * Marlow Greenan
 * Created: 09/01/2026
 * Last Updated: 09/18/2026 by Marlow Greenan
 * 
 * Contains the input controllers for a single scene.
 */

using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MarUtility.Multiplayer
{
    public class MultiplayerManager : Manager
    {
        private static MultiplayerManager inst;

        [SerializeField, OnValueChanged("OnVC_MaxPlayers")]
        private int _maxPlayerCount = 1;

        [SerializeField]
        private bool _forceSpawnPlayers = true;

        [SerializeField, OnValueChanged("OnVC_PlayerKey")]
        private string _playerKey = "Player";

        [SerializeField, Label("Controllers"), OnValueChanged("OnVC_MaxPlayers")]
        private Dictionary<string, PlayerInputController> _piControllers;

        #region GS
        public static MultiplayerManager INST { get => inst; }
        public Dictionary<string, PlayerInputController> PiControllers { get => _piControllers; set => _piControllers = value; }
        public int MaxPlayerCount { get => _maxPlayerCount; }
        #endregion

        public override void Initialize()
        {
            if (inst == null) inst = this;
            else DebugMessages.MultipleScriptInstances("Multiplayer Manager");

            if (MultiplayerMaster.INST != null)
                MultiplayerMaster.INST.UpdatePlayerCount();

            StartCoroutine(DelayedInitialize());

            base.Initialize();
        }

        private IEnumerator DelayedInitialize()
        {
            yield return new WaitForSeconds(0.1f);
            MultiplayerMaster.INST.SpawnAllPlayers();
        }

        //Returns the first unlinked controller.
        public KeyValuePair<string, PlayerInputController> GetFirstUnlinkedController()
        {
            foreach (KeyValuePair<string, PlayerInputController> pic in _piControllers)
                if (!pic.Value.IsLinked()) return pic;

            return new KeyValuePair<string, PlayerInputController>("", null);
        }

        public void EnableAllPlayerInput()
        {
            foreach (KeyValuePair<string, PlayerInputController> kvp in _piControllers)
                kvp.Value.EnableAllInput();
        }
        public void DisableAllPlayerInput()
        {
            foreach (KeyValuePair<string, PlayerInputController> kvp in _piControllers)
                kvp.Value.DisableAllInput();
        }

        #region Inspector
        private void OnVC_MaxPlayers()
        {
            if (_piControllers.Count <= _maxPlayerCount) return;

            while (_piControllers.Count > _maxPlayerCount)
                _piControllers.Remove(_piControllers.Last().Key);
        }

        private void OnVC_PlayerKey()
        {
            int i = 0;
            Dictionary<string, PlayerInputController> newDictionary = new Dictionary<string, PlayerInputController>();
            foreach (KeyValuePair<string, PlayerInputController> kvp in _piControllers)
            {
                newDictionary.Add(_playerKey + i, kvp.Value);
                i++;
            }
            _piControllers = newDictionary;
        }
        #endregion
    }
}

