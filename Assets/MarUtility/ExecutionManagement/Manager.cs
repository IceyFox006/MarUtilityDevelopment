/*
 * Marlow Greenan
 * Created: 4/19/2026
 * Last Updated: 6/20/2026
 * 
 *  A manager for the sceneloader system.
 */
using UnityEngine;

namespace MarUtility.ExecutionManagement
{
    public class Manager : MonoBehaviour
    {
        [SerializeField, Tooltip("When this is initialized.\nSCENELOADER_AWAKE is recommended to avoid execution order issues.")]
            private InitializeTime _initializeTime;

        #region GS
        public InitializeTime InitializeTime { get => _initializeTime; set => _initializeTime = value; }
        #endregion

        private void Awake()
        {
            if (_initializeTime == InitializeTime.AWAKE)
                Initialize();
        }
        private void Start()
        {
            if (_initializeTime == InitializeTime.START)
                Initialize();
        }

        public virtual void Initialize() { }
    }
}


