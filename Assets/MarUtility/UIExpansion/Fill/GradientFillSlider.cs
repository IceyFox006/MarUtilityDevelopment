/*
 * Marlow Greenan
 * Created: 6/20/2026
 * Last Updated: 6/27/2026
 * 
 * Changes the color of a slider based on how filled it is.
 */
using UnityEngine;
using UnityEngine.UI;

namespace MarUtility.UIExtensions
{
    [RequireComponent(typeof(FillController))]
    [RequireComponent(typeof(Slider))]
    public class GradientFillSlider : GradientFill
    {
        private Slider slider;

        protected override void Initialize()
        {
            base.Initialize();
            slider = GetComponent<Slider>();

            //Link slider events.
            slider.onValueChanged.AddListener(delegate { LinkSlider(); });
        }

        public override void Link()
        {
            slider.value = fm.FillAmount;
            base.Link();
        }
        private void LinkSlider()
        {
            fm.FillAmount = slider.value;
            base.Link();
        }

        #region Inspector
        public override void OnVC_FillAmount()
        {
            GetComponent<Slider>().value = GetComponent<FillController>().FillAmount;
            base.OnVC_FillAmount();
        }
        #endregion
    }
}

