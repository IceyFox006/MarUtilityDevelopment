/*
 * Marlow Greenan
 * Created: 6/30/2026
 * Last Updated: 8/28/2026 by Marlow Greenan
 * 
 * Manages the order in which managers are initialized.
 */
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace MarUtility.ExecutionManagement
{
    public class TransitionManager : Manager
    {
        private static TransitionManager inst;

        [SerializeField]
        private AnimatorController _ac;

        [SerializeField, BoxGroup("Animation IDs")]
        private string _trigOpenID = "T_Open";
        [SerializeField, BoxGroup("Animation IDs")]
        private string _trigCloseID = "T_Close";

        [SerializeField, BoxGroup("Events")]
        private UnityEvent _onOpenEnd;
        [SerializeField, BoxGroup("Events")]
        private UnityEvent _onCloseEnd;

        private string nextScene;

        #region GS
        public static TransitionManager INST { get => inst; }
        #endregion

        public override void Initialize()
        {
            if (inst == null)
                inst = this;
            else
                Debug.LogError("There are multiple instances of TRANSITION_MANAGER. You can only have one.");

            base.Initialize();
        }

        public void PlayOpen()
        {
            _ac.SetTrigger(_trigOpenID);
        }
        public void OnOpenEnd()
        {
            _onOpenEnd.Invoke();
        }

        public void PlayClose(string si)
        {
            _ac.SetTrigger(_trigCloseID);
            nextScene = si;
        }
        public void OnCloseEnd()
        {
            SceneManager.INST.LoadScene(nextScene);
            nextScene = "";
        }
    }
}

