using MarUtility.ExecutionManagement;
using UnityEngine;

namespace MarUtility
{
    public class EventMethod : MonoBehaviour
    {
        public void SceneTransition(int index)
        {
            //Play transistion
            GameManager.INSTANCE.LoadScene((SceneIndex)index); //end of transition
        }
    }

}

