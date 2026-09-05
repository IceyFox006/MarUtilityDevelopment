/*
 * Marlow Greenan
 * Created: 7/1/2026
 * Last Updated: 09/05/2026 by Marlow Greenan
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
        public void SceneTransition(string name)
            => TransitionManager.INST.PlayClose(name);
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

        #region Move
        public void PositionLerp(LerpPositionDataEndP lerpData)
            => lerpData.BeginPositionLerp();
        public void PositionLerp(LerpPositionDataEndT lerpData)
            => lerpData.BeginPositionLerp();
        #endregion
    }
}

