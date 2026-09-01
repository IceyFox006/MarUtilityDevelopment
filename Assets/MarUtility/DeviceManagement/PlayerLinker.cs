using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility.Multiplayer
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerLinker : MonoBehaviour
    {
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            transform.parent = MultiplayerManager.INST.transform;

            PlayerInputController piController = MultiplayerManager.INST.GetFirstUnlinkedController();
            if (piController != null) piController.Link(GetComponent<PlayerInput>());
        }
    }
}

