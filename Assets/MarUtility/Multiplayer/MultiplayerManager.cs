/*
 * Marlow Greenan
 * Created: 09/01/2026
 * Last Updated: 09/02/2026 by Marlow Greenan
 * 
 * Contains the input controllers for a single scene.
 */

using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace MarUtility.Multiplayer
{
    public class MultiplayerManager : Manager
    {
        private static MultiplayerManager inst;

        [SerializeField, Label("Controllers")]
        private Dictionary<string, PlayerInputController> _piControllers;

        #region GS
        public static MultiplayerManager INST { get => inst; }
        public Dictionary<string, PlayerInputController> PiControllers { get => _piControllers; set => _piControllers = value; }
        #endregion

        public override void Initialize()
        {
            if (inst == null) inst = this;
            else DebugMessages.MultipleScriptInstances("Multiplayer Manager");

            base.Initialize();
        }

        //Returns the first unlinked controller.
        public KeyValuePair<string, PlayerInputController> GetFirstUnlinkedController()
        {
            foreach (KeyValuePair<string, PlayerInputController> pic in _piControllers)
                if (!pic.Value.IsLinked()) return pic;

            return new KeyValuePair<string, PlayerInputController>("", null);
        }
    }
}

