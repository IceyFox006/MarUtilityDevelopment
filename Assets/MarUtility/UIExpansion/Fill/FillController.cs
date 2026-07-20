/*
 * Marlow Greenan
 * Created: 6/26/2026
 * Last Updated: 7/19/2026
 * 
 * Controls various UIExtensions related to the fill of images and sliders.
 */
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MarUtility.UIExtensions
{

    public class FillController : MonoBehaviour
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
        public LerpData Ld { get => _ld; set => _ld = value; }
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
            _ld.OnStart.Invoke();

            while (lTime < _ld.Duration)
            {
                FillAmount = Mathf.Lerp(lStart, lEnd, _ld.Curve.Evaluate(lTime / _ld.Duration));//FillAmount = Mathf.Lerp(lStart, lEnd, lTime / _ld.Duration);
                lTime += Time.deltaTime;
                _ld.OnBody.Invoke();
                yield return null;
            }
            FillAmount = lEnd;
            _ld.OnEnd.Invoke();
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

