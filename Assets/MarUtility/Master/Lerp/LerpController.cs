/*
 * Marlow Greenan
 * Created: 09/05/2026
 * Last Updated: 09/05/2026 by Marlow Greenan
 * 
 */
using System.Collections.Generic;
using UnityEngine;

namespace MarUtility
{
    public class LerpController : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<string, LerpPositionDataEndT> _positionLerps;

        public void PositionLerp(string lerpID)
            => _positionLerps[lerpID].BeginPositionLerp();
    }
}


