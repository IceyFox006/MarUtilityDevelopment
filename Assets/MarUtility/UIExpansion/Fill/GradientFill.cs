/*
 * Marlow Greenan
 * Created: 6/20/2026
 * Last Updated: 6/27/2026
 * 
 * Changes the color of a ui element based on how filled it is.
 */
using UnityEngine;
using UnityEngine.UI;

namespace MarUtility.UIExtensions
{
    [RequireComponent(typeof(FillController))]
    public class GradientFill : Fill
    {
        [SerializeField]
        protected Image _image;
        [SerializeField]
        private Gradient _gradient;

        #region GS
        public Gradient Gradient { get => _gradient; set => _gradient = value; }
        #endregion

        protected override void Initialize()
        {
            base.Initialize();
            fm.OnLink.AddListener(delegate {  Link(); });
        }
        public override void Link() 
            => _image.color = _gradient.Evaluate(fm.FillAmount);

        #region Inspector
        public override void OnVC_FillAmount()
            => _image.color = _gradient.Evaluate(GetComponent<FillController>().FillAmount);
        #endregion
    }
}


