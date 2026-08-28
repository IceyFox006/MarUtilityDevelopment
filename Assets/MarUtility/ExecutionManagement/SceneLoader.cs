/*
 * Marlow Greenan
 * Created: 4/19/2026
 * Last Updated: 8/12/2026
 * 
 * Manages the order in which managers are initialized.
 */
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MarUtility.ExecutionManagement
{
    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader instance;

        [SerializeField, MinValue(0.1f)]
            private float _tickInterval;
        [SerializeField]
            private bool _runTickUpdate = true;

        [SerializeField]
            private Manager[] _managers;

        #region GS
        public static SceneLoader INST { get => instance; private set => instance = value; }
        public float TickInterval
        {
            get => _tickInterval;
            set
            {
                _tickInterval = value;
                if (_tickInterval < 0) _tickInterval = 0;
            }
        }
        public bool RunTickUpdate { get => _runTickUpdate; set => _runTickUpdate = value; }
        #endregion

        private void Awake()
        {
            //Load persistant scene.
            if (!UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex((int)SceneIndex.PERSISTANT).isLoaded)
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)SceneIndex.PERSISTANT, LoadSceneMode.Additive);

            //SceneLoader Instance
            if (INST != null && INST != this)
                Destroy(this);
            else
                INST = this;

            //Initialize managers
            foreach (Manager manager in _managers)
                if (manager.InitializeTime == InitializeTime.SCENELOADER_AWAKE) manager.Initialize();
        }
    }
    public enum InitializeTime
    {
        MANUAL = 000,
        SCENELOADER_AWAKE = 100,
        AWAKE = 110,
        START = 200,
    }

    public enum UpdateTime
    {
        NONE = 000,
        SCENELOADER_TICK = 100,
        FIXED_UPDATE = 200,
        UPDATE = 300,
    }
}

