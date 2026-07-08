/*
 * Marlow Greenan
 * Created: 6/30/2026
 * Last Updated: 7/8/2026
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

        [SerializeField, BoxGroup("Animation")]
        private AnimatorController _ac;
        [SerializeField, BoxGroup("Animation")]
        private string _openTrigger;
        [SerializeField, BoxGroup("Animation")]
        private string _closeTrigger;

        [SerializeField]
        private UnityEvent _onOpenEnd;
        [SerializeField]
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

        public void Open()
        {
            _ac.SetTrigger(_openTrigger);
        }
        public void OnOpenEnd()
        {
            _onOpenEnd.Invoke();
        }

        public void Close(SceneIndex si)
        {
            _ac.SetTrigger(_closeTrigger);
            nextScene = (int)si;
        }
        public void OnCloseEnd()
        {
            SceneManager.INSTANCE.LoadScene((SceneIndex)nextScene);
            nextScene = -1;
        }
    }
}

