/*
 * Marlow Greenan
 * Created: 6/20/2026
 * Last Updated: 6/20/2026
 * 
 * Applies color regions to 2D (SpriteRenderer) objects.
 */
using NaughtyAttributes;
using System;
using UnityEngine;

namespace MarUtility.MaterialRegion
{
    [RequireComponent(typeof(SpriteRenderer))]
    [Serializable]
    public class RegionApplicator2D : RegionApplicator
    {
        [SerializeField, Required]
        private SpriteRenderer _renderer;

        protected override void Apply()
        {
            for (int i = 0; i < curRTs.Length; i++)
                MaterialRegionMaster.ChangeColorAtRegion(i, curRTs[i].RColor, _renderer);
        }
    }
}

