/*
 * Marlow Greenan
 * Created: 7/1/2026
 * Last Updated: 7/20/2026
 * 
 * General methods used for buttons, animation events, etc.
 */
using MarUtility.ExecutionManagement;
using UnityEngine;

namespace MarUtility
{
    public class EventMethod : MonoBehaviour
    {
        #region Debug
        public void DebugLog(string message)
            => Debug.Log(message);
        public void DebugLogError(string message)
            => Debug.Log(message);
        #endregion

        #region Scene
        public void SceneLoad(int index)
            => SceneManager.INST.LoadScene(index); //end of transition

        public void SceneTransition(int index)
            => TransitionManager.INST.PlayClose(index);
        #endregion

        #region Active
        public void SetActiveT(GameObject go)
            => go.SetActive(true);
        public void SetActiveF(GameObject go)
            => go.SetActive(false);
        #endregion

        #region Destroy
        //Destroys all children of the parent.
        public static void DestroyChildren(Transform parent)
        {
            for (int i = parent.childCount - 1; i > -1; i--)
                Destroy(parent.GetChild(i).gameObject);
        }
        #endregion

        #region Canvas
        public void ShowCanvasGroup(CanvasGroup cg)
            => cg.alpha = 1f;
        public void HideCanvasGroup(CanvasGroup cg)
            => cg.alpha = 0f;
        #endregion
    }
}

