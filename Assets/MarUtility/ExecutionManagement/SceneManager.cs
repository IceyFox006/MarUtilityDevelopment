/*
 * Marlow Greenan
 * Created: 6/27/2026
 * Last Updated: 6/27/2026
 * 
 * Manages loading scene & scene loading.
 */
using MarUtility.UIExtensions;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MarUtility.ExecutionManagement
{
    [Serializable]
    public enum SceneIndex
    {
        PERSISTANT = 0,
        TITLE = 1,
    }

    public class SceneManager : MonoBehaviour
    {
        public static SceneManager INSTANCE;

        [SerializeField, Required]
        private GameObject _loadingScreen;
        [SerializeField]
        private FillController _progressBarFill;
        [SerializeField]
        private TMP_Text _loadingText;

        private float sceneLoadPercent;
        
        private SceneIndex curScene;
        List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

        private void Awake()
        {
            if (INSTANCE == null)
                INSTANCE = this;
            else
                Debug.LogError("There are multiple instances of GAME_MANAGER. You can only have one.");

            curScene = (SceneIndex)UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }

        //Deloads current and loads new.
        public void LoadScene(SceneIndex si)
        {
            _loadingScreen.SetActive(true);
            scenesLoading.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)curScene));

            curScene = si;
            scenesLoading.Add(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)si, LoadSceneMode.Additive));
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
            if (TransitionManager.INSTANCE != null)
                TransitionManager.INSTANCE.Open();
        }
    }
}

