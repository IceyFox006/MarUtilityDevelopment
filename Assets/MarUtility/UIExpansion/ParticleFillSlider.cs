/*
 * Marlow Greenan
 * Created: 6/26/2026
 * Last Updated: 6/26/2026
 * 
 * Plays particles on fill change.
 */
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace MarUtility.UIExtensions 
{
    [RequireComponent(typeof(FillManager))]
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

        [SerializeField, EnumFlags, Tooltip("When particles will be played on lerping.")]
        private LerpEvent _lerpPlayOn;

        protected override void Initialize() 
        {
            base.Initialize();
            slider = GetComponent<Slider>();

            //Link lerp events
            if (_lerpPlayOn.HasFlag(LerpEvent.START))
                fm.OnLerpStart.AddListener(delegate { PlayParticle(); });
            if (_lerpPlayOn.HasFlag(LerpEvent.BODY))
                fm.OnLerpBody.AddListener(delegate {  PlayParticle(); });
            if (_lerpPlayOn.HasFlag(LerpEvent.END))
                fm.OnLerpEnd.AddListener(delegate {  PlayParticle(); });

            //Link slider events
            slider.onValueChanged.AddListener(delegate { LinkSlider(); });
        }

        private void LinkSlider()
        {
            if (_lerpPlayOn.HasFlag(LerpEvent.BODY))
                PlayParticle();
        }

        private void PlayParticle()
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


