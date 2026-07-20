/*
 * Marlow Greenan
 * Created: 6/26/2026
 * Last Updated: 6/27/2026
 * 
 * Parent class for fill based UI extentions.
 */
using UnityEngine;

namespace MarUtility.UIExtensions
{
    public class Fill : MonoBehaviour
    {
        protected FillController fm;
        private void Start()
        {
            Initialize();
            Link();
        }

        protected virtual void Initialize()
            => SetFM();
        protected virtual void SetFM()
            => fm = GetComponent<FillController>();
        public virtual void OnVC_FillAmount(){ }
        public virtual void Link() { }
    }
}

