/*
 * Marlow Greenan
 * Created: 4/19/2026
 * Last Updated: 6/21/2026
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

        [SerializeField, MinValue(0.001f)]
            private float _tickInterval;
        [SerializeField]
            private bool _runTickUpdate = true;

        [SerializeField]
            private Manager[] _managers;

        #region GS
        public static SceneLoader Instance { get => instance; private set => instance = value; }
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
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;

            //Initialize managers
            foreach (Manager manager in _managers)
                if (manager.InitializeTime == InitializeTime.SCENELOADER_AWAKE) manager.Initialize();
        }
    }
    public enum InitializeTime
    {
        NONE = 000,
        SCENELOADER_AWAKE = 100,
        AWAKE = 110,
        START = 200,
    }
}

