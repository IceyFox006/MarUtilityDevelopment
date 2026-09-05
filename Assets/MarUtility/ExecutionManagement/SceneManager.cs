/*
 * Marlow Greenan
 * Created: 6/27/2026
 * Last Updated: 6/27/2026
 * 
 * Manages loading scene & scene loading.
 */
using MarUtility.UIExtensions;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MarUtility.ExecutionManagement
{
    public class SceneManager : MonoBehaviour
    {
        private static SceneManager inst;

        [SerializeField, BoxGroup("Loading Screen")]
        private GameObject _loadingScreen;
        [SerializeField, BoxGroup("Loading Screen")]
        private FillController _progressBarFill;
        [SerializeField, BoxGroup("Loading Screen")]
        private TMP_Text _loadingText;

        [SerializeField]
        private UnityEvent _onSceneLoaded;

        private float sceneLoadPercent;
        
        private string curScene;
        List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

        #region GS
        public static SceneManager INST { get => inst; }
        #endregion

        private void Awake()
        {
            if (inst == null)
                inst = this;
            else
                Debug.LogError("There are multiple instances of GAME_MANAGER. You can only have one.");

            curScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        //Deloads current and loads new.
        public void LoadScene(string si)
        {
            _loadingScreen.SetActive(true);
            scenesLoading.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(curScene));

            curScene = si;
            scenesLoading.Add(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(si, LoadSceneMode.Additive));
            StartCoroutine(GetSceneLoadProgress());
        }

        //Updates loading progress bar.
        private IEnumerator GetSceneLoadProgress()
        {
            for (int i = 0; i < scenesLoading.Count; i++)
            {
                while (!scenesLoading[i].isDone)
                {
                    sceneLoadPercent = 0;

                    foreach (AsyncOperation ao in scenesLoading)
                    {
                        sceneLoadPercent += ao.progress;
                    }
                    sceneLoadPercent = (sceneLoadPercent / scenesLoading.Count);
                    _progressBarFill.FillAmount = sceneLoadPercent;

                    yield return null;
                }
            }
            _loadingScreen.gameObject.SetActive(false);

            _onSceneLoaded.Invoke();

            if (TransitionManager.INST != null)
                TransitionManager.INST.PlayOpen();
        }
    }
}

