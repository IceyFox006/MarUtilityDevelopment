/*
 * Marlow Greenan
 * Created: 8/13/2026
 * Last Updated: 8/14/2026
 * 
 * Manages various forms of inputs for the player.
 */
using MarUtility.ExecutionManagement;
using MarUtility.UIExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarUtility.DeviceManagement
{
    public class PlayerInputController : Manager
    {
        [SerializeField]
        private SO_Player _player;
        [SerializeField]
        private PlayerInput _input;
        [SerializeField]
        private ObjectEventSystem _oes;

        #region GS
        public PlayerInput Input { get => _input; }
        public SO_Player Player { get => _player; }
        #endregion

        public override void Initialize()
        {
            _oes.Initialize();

            base.Initialize();
        }

        public void EnableAllInput()
        {
            _input.enabled = true;
            _oes.ReceiveInput = true;
        }
        public void DisableAllInput()
        {
            _input.enabled = false;
            _oes.ReceiveInput = false;
        }
    }
}

