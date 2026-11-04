/*
 * Marlow Greenan
 * Created: 09/05/2026
 * Last Updated: 09/05/2026 by Marlow Greenan
 * 
 */
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace MarUtility
{
    public class LerpController : MonoBehaviour
    {
        //POSITION
        [SerializeField, BoxGroup("Position")]
        private Dictionary<string, LerpPositionDataEndT> _positionLerps;

        //COLOR
        [SerializeField, BoxGroup("Color")]
        private Dictionary<string, LerpColorDataI> _imageColorLerps;

        public void PositionLerp(string lerpID)
            => _positionLerps[lerpID].BeginPositionLerp();

        public void ImageColorLerp(string lerpID)
            => _imageColorLerps[lerpID].BeginColorLerp();



    }
}


