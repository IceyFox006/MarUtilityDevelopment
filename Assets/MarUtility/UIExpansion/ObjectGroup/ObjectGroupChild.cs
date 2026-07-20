/*
 * Marlow Greenan
 * Created: 7/19/2026
 * Last Updated: 7/19/2026
 * 
 * Controls movement of ObjectGroup children.
 */
using NaughtyAttributes;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace MarUtility.UIExtensions
{
    public class ObjectGroupChild : MonoBehaviour
    {
        //RETURN
        [SerializeField, Label("Return Lerp Data"), BoxGroup("Return")]
        private LerpData _returnLD;

        //ENTRANCE
        [SerializeField, BoxGroup("Entrance")]
        private bool _doEntrance;
        [SerializeField, ShowIf("_doEntrance"), BoxGroup("Entrance"), Tooltip("The position added to the origin point to get the true start point.\n_entrancePos + originPos = trueStart")]
        private Vector3 _entrancePos;
        [SerializeField, ShowIf("_doEntrance"), Label("Entrance Lerp Data"), BoxGroup("Entrance")]
        private LerpData _entranceLD;

        //EXIT
        [SerializeField, BoxGroup("Exit")]
        private bool _doExit;
        [SerializeField, ShowIf("_doExit"), BoxGroup("Exit"), Tooltip("The position added to the origin point to get the true start point.\n_exitPos + originPos = trueStart")]
        private Vector3 _exitPos;
        [SerializeField, ShowIf("_doExit"), Label("Exit Lerp Data"), BoxGroup("Exit")]
        private LerpData _exitLD;

        //LERP
        private Vector3 lStart;
        private Vector3 lEnd;
        private float lTime;
        private LerpData curLD;
        private bool isLerping;

        private Vector3 originPos;

        #region GS
        public Vector3 OriginPos { get => originPos; set => originPos = value; }
        public bool IsLerping { get => isLerping; set => isLerping = value; }
        #endregion

        public void Initialize()
        {
            originPos = transform.position;
            if (_doEntrance)
                transform.position += _entrancePos;
        }

        #region PositionLerp
        public void BeginReturnToOriginLerp()
            => BeginPositionLerp(_returnLD, originPos);

        //Begins lerping the entrance.
        public void BeginEntranceLerp()
            => BeginPositionLerp(_entranceLD, originPos + _entrancePos, originPos);

        //Begins lerping the exit.
        public void BeginExitLerp()
            => BeginPositionLerp(_exitLD, originPos, originPos + _exitPos);

        //Begins lerping the position.
        public void BeginPositionLerp(LerpData cLD, Vector3 end)
            => BeginPositionLerp(cLD, transform.position, end);
        public void BeginPositionLerp(LerpData cLD, Vector3 start, Vector3 end)
        {
            curLD = cLD;
            lStart = start;
            lEnd = end;

            try { StartCoroutine(PositionLerpInterval()); }
            catch(MissingReferenceException){}
        }

        //Lerps the objects position.
        private IEnumerator PositionLerpInterval()
        {
            lTime = 0;
            transform.position = lStart;
            curLD.OnStart.Invoke();
            isLerping = true;

            while (lTime < curLD.Duration)
            {
                transform.position = Vector3.Lerp(lStart, lEnd, lTime / curLD.Duration);
                lTime += Time.deltaTime;
                curLD.OnBody.Invoke();
                yield return null;
            }

            transform.position = lEnd;
            curLD.OnEnd.Invoke();
            isLerping = false;
        }
        #endregion

        #region Inspector
        [Button]
        private void SimulateReturn()
        {
            if (!EditorApplication.isPlaying)
            {
                DebugMessages.SimulationPlaytestOnly("Return");
                return;
            }
            BeginReturnToOriginLerp();
        }

        [Button]
        private void SimulateEntrance()
        {
            if (!EditorApplication.isPlaying)
            {
                DebugMessages.SimulationPlaytestOnly("Entrance");
                return;
            }
            BeginEntranceLerp();
        }

        [Button]
        private void SimulateExit()
        {
            if (!EditorApplication.isPlaying)
            {
                DebugMessages.SimulationPlaytestOnly("Exit");
                return;
            }
            BeginExitLerp();
        }
        #endregion

    }
}


