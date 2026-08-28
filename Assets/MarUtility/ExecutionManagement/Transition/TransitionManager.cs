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
        public static TransitionManager INSTANCE;

        [SerializeField]
        private AnimatorController _ac;
        [SerializeField, BoxGroup("Parameter IDs")]
        private string _trigOpenID = "T_Open";
        [SerializeField, BoxGroup("Parameter IDs")]
        private string _trigCloseID = "T_Close";

        [SerializeField, BoxGroup("Events")]
        private UnityEvent _onOpenEnd;
        [SerializeField, BoxGroup("Events")]
        private UnityEvent _onCloseEnd;
        private int nextScene;

        public override void Initialize()
        {
            if (INSTANCE == null)
                INSTANCE = this;
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

        public void Close(SceneIndex si)
        {
            _ac.SetTrigger(_trigCloseID);
            nextScene = (int)si;
        }
        public void OnCloseEnd()
        {
            SceneManager.INSTANCE.LoadScene((SceneIndex)nextScene);
            nextScene = -1;
        }
    }
}

