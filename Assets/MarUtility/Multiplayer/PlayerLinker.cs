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

        public void Relink()
        {
            PlayerInputController piCon = MultiplayerManager.INST.PiControllers[keyID];

            if (piCon == null) return;
            
            piCon.Link(pi);

        }
    }
}

