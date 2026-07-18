/*
 * Marlow Greenan
 * Created: 6/20/2026
 * Last Updated: 6/21/2026
 * 
 * Contains methods for tint, shade, and hue shifting.
 */
using NaughtyAttributes;
using UnityEngine;

namespace MarUtility
{
    public class ColorShifter : MonoBehaviour
    {
        //SAMPLER
        //Percentage
        [SerializeField, Range(0, 0.3f), BoxGroup("Sampler"), OnValueChanged("OnVC_Percent"), Tooltip("The percent hue difference (Hue shifting).")]
        private float hPercent = 0.1f;
        [SerializeField, Range(0, 0.3f), BoxGroup("Sampler"), OnValueChanged("OnVC_Percent"), Tooltip("The percent value differece (Tint/shade).")]
        private float vPercent = 0.1f;

        //Colors
        [ReadOnly, SerializeField, BoxGroup("Sampler")]
        private Color cshade3 = new Color (0.2f, 0.2f, 0.2f, 1);
        [ReadOnly, SerializeField, BoxGroup("Sampler")]
        private Color cshade2 = new Color(0.3f, 0.3f, 0.3f, 1);
        [ReadOnly, SerializeField, BoxGroup("Sampler")]
        private Color cshade1 = new Color(0.4f, 0.4f, 0.4f, 1);
        [SerializeField, BoxGroup("Sampler"), OnValueChanged("OnVC_Percent")]
        private Color c = new Color(0.5f, 0.5f, 0.5f, 1);
        [ReadOnly, SerializeField, BoxGroup("Sampler")]
        private Color ctint1 = new Color(0.6f, 0.6f, 0.6f, 1);
        [ReadOnly, SerializeField, BoxGroup("Sampler")]
        private Color ctint2 = new Color(0.7f, 0.7f, 0.7f, 1);
        [ReadOnly, SerializeField, BoxGroup("Sampler")]
        private Color ctint3 = new Color(0.8f, 0.8f, 0.8f, 1);


        //Hue shift
        public static Color GetHueShift(Color c, float hPercent)
            => GetHueShift(c, hPercent, 0.10f);
        public static Color GetHueShift(Color c, float hPercent, float vsPercent)
        {
            Vector3 hsv;
            Color.RGBToHSV(c, out hsv.x, out hsv.y, out hsv.z);

            hsv.x = Math.LoopAround(hsv.x + hPercent, 0, 1f); //Mathf.Clamp(hsv.x + percent, 0, 1f);
            hsv.y = Mathf.Clamp(hsv.y - (Math.BaseOne(hPercent) * vsPercent), 0, 1f);
            hsv.z = Mathf.Clamp(hsv.z + (Math.BaseOne(hPercent) * vsPercent), 0, 1f);

            return Color.HSVToRGB(hsv.x, hsv.y, hsv.z);
        }

        //Tint
        public static Color GetTint(Color c, float percent)
            => new Color(CalculateTint(c.r, percent), CalculateTint(c.g, percent), CalculateTint(c.b, percent));
        private static float CalculateTint(float rgb, float percent)
            => Mathf.Clamp(rgb + percent, 0, 1f);

        //Shade
        public static Color GetShade(Color c, float percent)
            => new Color(CalculateShade(c.r, percent), CalculateShade(c.g, percent), CalculateShade(c.b, percent));
        private static float CalculateShade(float rgb, float percent)
            => Mathf.Clamp(rgb - percent, 0, 1f);

        #region Inspector
        [Button]
        private void Randomize()
        {
            c = Math.RandomColor();
            hPercent = Random.Range(0, 0.3f);
            vPercent = Random.Range(0, 0.3f);

            OnVC_Percent();
        }
        private void OnVC_Percent()
        {
            if (hPercent == 0)
            {
                cshade1 = GetShade(c, vPercent);
                cshade2 = GetShade(c, 2 * vPercent);
                cshade3 = GetShade(c, 3 * vPercent);

                ctint1 = GetTint(c, vPercent);
                ctint2 = GetTint(c, 2 * vPercent);
                ctint3 = GetTint(c, 3 * vPercent);
            }
            else
            {
                cshade1 = GetHueShift(c, -hPercent, vPercent);
                cshade2 = GetHueShift(c, -(2 * hPercent), 2 * vPercent);
                cshade3 = GetHueShift(c, -(3 * hPercent), 3 * vPercent);

                ctint1 = GetHueShift(c, hPercent, vPercent);
                ctint2 = GetHueShift(c, 2 * hPercent, 2 * vPercent);
                ctint3 = GetHueShift(c, 3 * hPercent, 3 * vPercent);
            }


        }
        #endregion
    }
}

