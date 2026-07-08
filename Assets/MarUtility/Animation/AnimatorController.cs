/*
 * Marlow Greenan
 * Created: 7/1/2026
 * Last Updated: 7/8/2026
 * 
 * Manages the order in which managers are initialized.
 */
using System.Collections;
using UnityEngine;

namespace MarUtility.ExecutionManagement
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorController : MonoBehaviour
    {
        private Animator _animator;

        private string curClip;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayAnimation(string an)
        {
            curClip = an;
            _animator.Play(an);
        }

        public void SetTrigger(string trigger)
            => _animator.SetTrigger(trigger);

        private IEnumerator AnimationCD()
        {
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);

        }
    }
}
