/*
 * Marlow Greenan
 * Created: 09/01/2026
 * Last Updated: 09/02/2026
 * 
 * On a player input player. Remembers the player it was initially assigned to and when switching scenes, relinks to PiController under the same keyID.
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;
using MarUtility.ExecutionManagement;

namespace MarUtility.Multiplayer
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerLinker : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private string keyID;

        [SerializeField]
        private bool _autoLinkOnSceneLoad = true;

        private PlayerInput pi;
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            transform.parent = MultiplayerMaster.INST.transform;
            pi = GetComponent<PlayerInput>();

            if (_autoLinkOnSceneLoad)
            {
                KeyValuePair<string, PlayerInputController> piPair = MultiplayerManager.INST.GetFirstUnlinkedController();

                if (piPair.Value == null) return;

                keyID = piPair.Key;
                piPair.Value.Link(pi);

                TransitionManager.INST.OnOpenEnd.AddListener(delegate { Relink(); } );
            }
        }

        //Links to the piController with the same keyID.
        public void Relink()
        {
            if (!_autoLinkOnSceneLoad) return;

            PlayerInputController piCon = MultiplayerManager.INST.PiControllers[keyID];

            if (piCon == null) return;
            
            piCon.Link(pi);

        }
    }
}

