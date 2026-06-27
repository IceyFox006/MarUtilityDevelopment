using NaughtyAttributes;
using UnityEngine;

namespace MarUtility
{
    public class LerpMaster : MonoBehaviour
    {

    }

    [System.Serializable]
    public class LerpData
    {
        [SerializeField, MinValue(0.001f)]
        private float _duration = 1;
        [SerializeField, CurveRange(EColor.Green)]
        private AnimationCurve _curve;

        #region GS
        public float Duration { get => _duration; set => _duration = value; }
        public AnimationCurve Curve { get => _curve; set => _curve = value; }
        #endregion
    }
}

