/*
 * Marlow Greenan
 * Created: 6/26/2026
 * Last Updated: 6/27/2026
 * 
 * Managers various UIExtensions related to the fill of images and sliders.
 */
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MarUtility.UIExtensions
{

    public class FillManager : MonoBehaviour
    {
        [SerializeField, Range(0, 1), OnValueChanged("OnVC_FillAmount")]
        protected float _fillAmount = 1;

        //LERP
        [SerializeField, Label("Lerp Data")]
        private LerpData _ld;
        private float lStart;
        private float lEnd;
        private float lTime;

        //EVENTS
        private UnityEvent onLink = new UnityEvent();
        private UnityEvent onLerpStart = new UnityEvent();
        private UnityEvent onLerpBody = new UnityEvent();
        private UnityEvent onLerpEnd = new UnityEvent();

        #region GS
        public virtual float FillAmount
        {
            get => _fillAmount;
            set
            {
                _fillAmount = Mathf.Clamp(value, 0, 1f);
                onLink.Invoke();
            }
        }

        public UnityEvent OnLink { get => onLink; set => onLink = value; }
        public UnityEvent OnLerpStart { get => onLerpStart; set => onLerpStart = value; }
        public UnityEvent OnLerpBody { get => onLerpBody; set => onLerpBody = value; }
        public UnityEvent OnLerpEnd { get => onLerpEnd; set => onLerpEnd = value; }
        #endregion

        //Begins lerping the fill amount.
        public void BeginLerp(float end)
            => BeginLerp(_fillAmount, end);
        public void BeginLerp(float start, float end)
        {
            lStart = start;
            lEnd = end;
            StartCoroutine(LerpInterval());
        }

        //Lerps the fill amount.
        private IEnumerator LerpInterval()
        {
            lTime = 0;
            FillAmount = lStart;
            onLerpStart.Invoke();

            while (lTime < _ld.Duration)
            {
                FillAmount = Mathf.Lerp(lStart, lEnd, _ld.Curve.Evaluate(lTime / _ld.Duration));//FillAmount = Mathf.Lerp(lStart, lEnd, lTime / _ld.Duration);
                lTime += Time.deltaTime;
                onLerpBody.Invoke();
                yield return null;
            }
            FillAmount = lEnd;
            onLerpEnd.Invoke();
        }

        #region Inspector
        [Button("Simulate")]
        private void TestLerp()
            => BeginLerp(1, 0);

        private void OnVC_FillAmount()
        {
            Fill[] fs = GetComponents<Fill>();
            foreach (Fill f in fs)
                f.OnVC_FillAmount();
        }
        #endregion
    }
}

