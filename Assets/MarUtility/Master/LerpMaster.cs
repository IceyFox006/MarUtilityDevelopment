/*
 * Marlow Greenan
 * Created: 6/26/2026
 * Last Updated: 7/19/2026
 * 
 * Contains data for a lerp action.
 */
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace MarUtility
{
    public class LerpMaster : MonoBehaviour
    {

    }

    [System.Serializable]
    public class LerpData
    {
        //VISUAL
        [SerializeField, MinValue(0.001f)]
        private float _duration = 1;
        [SerializeField, CurveRange(EColor.Green)]
        private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        //EVENTS
        [SerializeField]
        private UnityEvent _onStart;
        [SerializeField]
        private UnityEvent _onBody;
        [SerializeField]
        private UnityEvent _onEnd;

        #region GS
        public float Duration { get => _duration; set => _duration = value; }
        public AnimationCurve Curve { get => _curve; set => _curve = value; }
        public UnityEvent OnStart { get => _onStart; set => _onStart = value; }
        public UnityEvent OnBody { get => _onBody; set => _onBody = value; }
        public UnityEvent OnEnd { get => _onEnd; set => _onEnd = value; }
        #endregion
    }
}

