/*
 * Marlow Greenan
 * Created: 7/1/2026
 * Last Updated: 7/8/2026
 * 
 * General methods used for buttons, animation events, etc.
 */
using MarUtility.ExecutionManagement;
using UnityEngine;

namespace MarUtility
{
    public class EventMethod : MonoBehaviour
    {
        public void SceneLoad(int index)
        {
            SceneManager.INSTANCE.LoadScene((SceneIndex)index); //end of transition
        }

        public void SceneTransition(int index)
        {
            TransitionManager.INSTANCE.Close((SceneIndex)index);
        }
    }
}

