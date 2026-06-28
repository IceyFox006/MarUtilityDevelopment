/*
 * Marlow Greenan
 * Created: 6/27/2026
 * Last Updated: 6/27/2026
 * 
 * Allows events to be play at the start, body, and end of all audio managed by the AudioMaster.
 */
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MarUtility
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceEvents : MonoBehaviour
    {
        private AudioSource source;

        [SerializeField]
        private bool _destroyOnEnd;

        //COROUTINE
        private float curTime;
        private float endTime;

        //EVENTS
        [SerializeField]
        private UnityEvent _onPlayStart;
        [SerializeField]
        private UnityEvent _onPlayBody;
        [SerializeField]
        private UnityEvent _onPlayEnd;

        #region GS
        public UnityEvent OnPlayEnd { get => _onPlayEnd; set => _onPlayEnd = value; }
        public AudioSource Source { get => source; set => source = value; }
        public float EndTime { get => endTime; set => endTime = value; }
        #endregion

        private void Start()
        {
            source = GetComponent<AudioSource>();
            if (_destroyOnEnd)
                _onPlayEnd.AddListener(delegate { Destroy(this); });
        }
        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public void BeginPlay()
        {
            source.Play();

            StartCoroutine(PlayInterval());
        }

        private IEnumerator PlayInterval()
        {
            curTime = 0;
            _onPlayStart.Invoke();

            while (curTime < endTime)
            {
                curTime += Time.deltaTime;

                _onPlayBody.Invoke();
                yield return null;
            }

            _onPlayEnd.Invoke();
        }
    }
}

