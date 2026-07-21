/*
 * Marlow Greenan
 * Created: 6/20/2026
 * Last Updated: 6/20/2026
 * 
 * Contains data for shader graph region application and management.
 */
using NaughtyAttributes;
using System;
using UnityEngine;

namespace MarUtility.Material
{
    public  static class MaterialRegionMaster
    {
        public static string[] VN_COLORS = new string[] { "_COLOR0", "_COLOR1", "_COLOR2", "_COLOR3", "_COLOR4", "_COLOR5" };

        //Changes the color at the regID of a sprite renderer.
        public static void ChangeColorAtRegion(int regID, Color newColor, SpriteRenderer renderer)
            => ChangeColorAtRegion(regID, newColor, renderer.sharedMaterial);
        
        //Changes the color at regID of a mesh renderer. 
        public static void ChangeColorAtRegion(int regID, Color newColor, MeshRenderer renderer)
            => ChangeColorAtRegion(regID, newColor, renderer.sharedMaterial);

        //Changes the color at regID of mat.
        public static void ChangeColorAtRegion(int regID, Color newColor, UnityEngine.Material mat)
        {
            if (!mat.HasColor(VN_COLORS[regID]))
            {
                Debug.LogError(mat.ToString() + " does not contain the color variable " + VN_COLORS[regID] + ".");
                return;
            }

            mat.SetColor(VN_COLORS[regID], newColor);
        }
    }

    //=================================================================================================================
    [Serializable]
    public class RegionType
    {
        private Color rColor;

        #region GS
        public Color RColor { get => rColor; set => rColor = value; }
        #endregion

        public virtual Color Randomize()
            => RColor;
    }

    //-----------------------------------------------------------------------------------------------------------------
    [Serializable]
    public class RegionTypeColorPool : RegionType
    {
        [SerializeField]
            private Color[] _colorPool;

        public override Color Randomize()
        {
            if (_colorPool.Length == 0)
                RColor = Color.black;
            else
                RColor = _colorPool[UnityEngine.Random.Range(0, _colorPool.Length)];
            return base.Randomize();
        }
    }

    //-----------------------------------------------------------------------------------------------------------------
    [Serializable]
    public class RegionTypeGradient : RegionType
    {
        [SerializeField]
            private Gradient _gradient;

        public override Color Randomize()
        {
            RColor = _gradient.Evaluate(UnityEngine.Random.Range(0f, 1f));
            return base.Randomize();
        }
    }

    //=================================================================================================================
    [Serializable]
    public class RegionApplicator : MonoBehaviour
    {
        [SerializeField]
            private ERegionType _regionData;

        [SerializeField, ShowIf("_regionData", ERegionType.COLOR_POOL), OnValueChanged("OnVC_RtColorPools")]
            private RegionTypeColorPool[] _rtColorPools;
        [SerializeField, ShowIf("_regionData", ERegionType.GRADIENT), OnValueChanged("OnVC_RtGradients")]
            private RegionTypeGradient[] _rtGradients;
        protected RegionType[] curRTs;

        private void Start()
        {
            Initialize();
        }

        [Button("Simulate")]
        public void Initialize()
        {
            switch (_regionData)
            {
                case ERegionType.COLOR_POOL: curRTs = _rtColorPools; break;
                case ERegionType.GRADIENT: curRTs = _rtGradients; break;
            }

            Randomize();
            Apply();
        }
        protected virtual void Apply() {}
        protected void Randomize()
        {
            foreach (RegionType rt in curRTs)
                rt.Randomize();
        }

        #region Inspector
        //Limit curRT length to number of color regions.
        private void OnVC_RtColorPools()
        {
            if (_rtColorPools.Length > MaterialRegionMaster.VN_COLORS.Length)
            {
                RegionTypeColorPool[] newRTs = new RegionTypeColorPool[MaterialRegionMaster.VN_COLORS.Length];
                for (int i = 0; i < newRTs.Length; i++)
                    newRTs[i] = _rtColorPools[i];
                _rtColorPools = newRTs;
            }
        }

        private void OnVC_RtGradients()
        {
            if (_rtGradients.Length > MaterialRegionMaster.VN_COLORS.Length)
            {
                RegionTypeGradient[] newRTs = new RegionTypeGradient[MaterialRegionMaster.VN_COLORS.Length];
                for (int i = 0; i < newRTs.Length; i++)
                    newRTs[i] = _rtGradients[i];
                _rtGradients = newRTs;
            }
        }
        #endregion
    }

    //=================================================================================================================
    public enum ERegionType
    {
        COLOR_POOL,
        GRADIENT,
    }
}


