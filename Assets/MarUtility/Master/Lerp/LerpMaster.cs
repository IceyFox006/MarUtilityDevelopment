/*
 * Marlow Greenan
 * Created: 6/26/2026
 * Last Updated: 9/05/2026 by Marlow Greenan
 * 
 * Contains data for a lerp action.
 */
using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MarUtility
{
    public class LerpMaster : MonoBehaviour {}

    [Flags]
    public enum LerpEvent
    {
        NONE = 0 << 000,
        START = 1 << 100,
        BODY = 1 << 200,
        END = 1 << 300,
    }

    [Serializable]
    public class LerpData
    {
        //VISUAL
        [SerializeField, MinValue(0.001f)]
        protected float _duration = 1;
        [SerializeField, CurveRange(EColor.Green)]
        private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        //EVENTS
        [SerializeField]
        protected UnityEvent _onStart;
        [SerializeField]
        protected UnityEvent _onBody;
        [SerializeField]
        protected UnityEvent _onEnd;

        protected float lTime;

        #region GS
        public float Duration { get => _duration; set => _duration = value; }
        public AnimationCurve Curve { get => _curve; set => _curve = value; }
        public UnityEvent OnStart { get => _onStart; set => _onStart = value; }
        public UnityEvent OnBody { get => _onBody; set => _onBody = value; }
        public UnityEvent OnEnd { get => _onEnd; set => _onEnd = value; }
        #endregion
    }
    #region Position
    //=================================================================================================================
    // Lerp Position Data
    //=================================================================================================================
    [Serializable]
    public class LerpPositionData : LerpData
    {
        [SerializeField, Required]
        private Transform _movingTransform;

        protected Vector3 lStart;
        protected Vector3 lEnd;
        
        public  virtual void BeginPositionLerp()
            => SceneManager.INST.StartCoroutine(PositionLerpInterval());

        //Lerps the _movingTransform's position.
        public IEnumerator PositionLerpInterval()
        {
            lTime = 0;
            lStart = _movingTransform.localPosition;
            _onStart.Invoke();

            while (lTime < _duration)
            {
                _movingTransform.localPosition = Vector3.Lerp(lStart, lEnd, lTime / _duration);
                lTime += Time.deltaTime;
                _onBody.Invoke();
                yield return null;
            }
            _movingTransform.localPosition = lEnd;
            _onEnd.Invoke();
        }
    }
    //-----------------------------------------------------------------------------------------------------------------
    [Serializable]
    public class LerpPositionDataEndT : LerpPositionData
    {
        [SerializeField]
        private Transform _endTransform;

        public override void BeginPositionLerp()
        {
            lEnd = _endTransform.localPosition;
            base.BeginPositionLerp();
        }
    }
    //-----------------------------------------------------------------------------------------------------------------
    [Serializable]
    public class LerpPositionDataEndP : LerpPositionData
    {
        [SerializeField]
        private Vector3 _endPosition;

        public Vector3 EndPosition { get => _endPosition; set => _endPosition = value; }

        public override void BeginPositionLerp()
        {
            lEnd = _endPosition;
            base.BeginPositionLerp();
        }
    }
    #endregion
    //=================================================================================================================
    // Lerp Image Color Data
    //=================================================================================================================
    [Serializable]
    public class LerpColorData : LerpData
    {
        [SerializeField]
        private Color _endColor = Color.white;

        protected Color lStart;
        protected Color lEnd;

        public virtual void BeginColorLerp()
        {
            lEnd = _endColor;
             SceneManager.INST.StartCoroutine(ColorLerpInterval());
        }
        protected virtual IEnumerator ColorLerpInterval() { yield return null; }
    }
    //-----------------------------------------------------------------------------------------------------------------
    [Serializable]
    public class LerpColorDataI : LerpColorData
    {
        [SerializeField]
        private Image _image;

        protected override IEnumerator ColorLerpInterval()
        {
            lTime = 0;
            lStart = _image.color;
            _onStart.Invoke();
            while (lTime < _duration)
            {
                _image.color = Color.Lerp(lStart, lEnd, lTime / _duration);
                lTime += Time.deltaTime;
                _onBody.Invoke();
                yield return null;
            }
            _image.color = lEnd;
            _onEnd.Invoke();
        }
    }
}

