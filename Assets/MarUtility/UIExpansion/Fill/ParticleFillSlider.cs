/*
 * Marlow Greenan
 * Created: 6/26/2026
 * Last Updated: 7/19/2026
 * 
 * Plays particles on fill change.
 */
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace MarUtility.UIExtensions 
{
    [RequireComponent(typeof(FillController))]
    [RequireComponent(typeof(Slider))]
    public class ParticleFillSlider : Fill
    {
        protected Slider slider;

        [SerializeField]
        private bool spawnsNew;
        [SerializeField, ShowIf("spawnsNew"), Tooltip("The name of the particle in the PARTICLE MASTER library. Case sensitive.")]
        private string _particleName;
        [SerializeField, HideIf("spawnsNew")]
        private ParticleSystem _particles;

        [SerializeField, Tooltip("If particles play when the player manually moves the slider.")]
        private bool _playOnInput = true;

        protected override void Initialize() 
        {
            base.Initialize();
            slider = GetComponent<Slider>();

            //Link slider events
            slider.onValueChanged.AddListener(delegate { LinkSlider(); });
        }

        private void LinkSlider()
        {
            if (_playOnInput)
                PlayParticle();
        }

        public void PlayParticle()
        {
            Vector3 pos = slider.handleRect.position;
            if (!spawnsNew)
            {
                _particles.transform.position = pos;
                _particles.Play();
            }
            else
                ParticleMaster.INSTANCE.Play(_particleName, pos);
        }
    }
}


