/*
 * Marlow Greenan
 * Created: 6/20/2026
 * Last Updated: 6/20/2026
 * 
 * Applies color regions to 3D (MeshRenderer) objects.
 */
using NaughtyAttributes;
using System;
using UnityEngine;

namespace MarUtility.Material
{
    [RequireComponent(typeof(MeshRenderer))]
    [Serializable]
    public class RegionApplicator3D : RegionApplicator
    {
        [SerializeField, Required]
        private MeshRenderer _renderer;

        protected override void Apply()
        {
            for (int i = 0; i < curRTs.Length; i++)
                MaterialRegionMaster.ChangeColorAtRegion(i, curRTs[i].RColor, _renderer);
        }
    }
}

