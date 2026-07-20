/*
 * Marlow Greenan
 * Created: 6/20/2026
 * Last Updated: 6/27/2026
 * 
 * Changes the color of a image based on how filled it is.
 */
using UnityEngine;
using UnityEngine.UI;

namespace MarUtility.UIExtensions
{
    [RequireComponent(typeof(FillController))]
    [RequireComponent(typeof(Image))]
    public class GradientFillImage : GradientFill
    {
        public override void Link()
        {
            base.Link();
            _image.fillAmount = fm.FillAmount;
        }

        #region Inspector
        public override void OnVC_FillAmount()
        {
            _image.fillAmount = GetComponent<FillController>().FillAmount;
            base.OnVC_FillAmount();
        }
        #endregion
    }
}


