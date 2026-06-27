/*
 * Marlow Greenan
 * Created: 6/27/2026
 * Last Updated: 6/27/2026
 * 
 * Manages all instantiated audio.
 */
using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace MarUtility
{
    public class AudioMaster : Manager
    {
        public static AudioMaster AUDIO_MASTER;

        [SerializeField, Required]
        private GameObject _audioDataSourcePrefab;

        [SerializeField, Tooltip("Keys are case sensitive. If a key is changed, make sure all instances where it is used are updated.")]
        private Dictionary<string, AudioData> _library;

        [SerializeField, BoxGroup("Simulate")]
        private AudioSource _testSource;
        [SerializeField, BoxGroup("Simulate")]
        private string _testName;

        public override void Initialize()
        {
            //Set singleton.
            if (AUDIO_MASTER == null)
                AUDIO_MASTER = this;
            else
                Debug.LogError("Multiple instances of AUDIO_MASTER exists, you can only have one.");

            //Spawn audio sources.
            foreach (AudioData ad in _library.Values)
                InstantiateSource(ad);

            base.Initialize();
        }

        //Adds and links source components for each audio data in the library to the audio master.
        private void InstantiateSource(AudioData aData)
            => aData.LinkToSource(gameObject.AddComponent<AudioSource>(), false);

        //Plays a sound at position.
        public void Play(string aName)
        {
            if (!_library.ContainsKey(aName))
                Debug.LogError("AUDIO_MASTER does not contain " + aName + ".");

            AudioData ad; _library.TryGetValue(aName, out ad);
            ad.Play();
        }

        //Spawns a source that plays a sound then is deleted.
        public GameObject PlayAt(string aName, Vector2 position)
            => PlayAt(aName, new Vector3(position.x, position.y, 0));
        public GameObject PlayAt(string aName, Vector3 position)
        {
            if (!_library.ContainsKey(aName))
                Debug.LogError("AUDIO_MASTER does not contain " + aName + ".");

            AudioData ad; _library.TryGetValue(aName, out ad);
            GameObject go = Instantiate(_audioDataSourcePrefab, position, Quaternion.identity);
            ad.Play(go.GetComponent<AudioSourceEvents>());

            return go;
        }

        #region Inspector
        [Button("Simulate")]
        private void TestSound()
        {
            AudioData ad; _library.TryGetValue(_testName, out ad);
            ad.LinkToSource(_testSource, true);
            Play(_testName);
        }
        #endregion
    }
    //=================================================================================================================
    [System.Serializable]
    public class AudioData
    {
        [SerializeField]
        private AudioClip _clip;

        [SerializeField, MinValue(0), Tooltip("The point in the clip where it begins playing.")]
        private float startTime = 0;
        [SerializeField, Tooltip("The point in the clip when play ends.\n-1 = CLIP LENGTH")]
        private float endTime = -1;

        [SerializeField, MinMaxSlider(0, 1)]
        private Vector2 _volumeRange;
        [SerializeField, MinMaxSlider(-3, 3)]
        private Vector2 _pitchRange;

        private AudioSource source;

        #region GS
        public AudioSource Source { get => source; set => source = value; }
        #endregion

        public void LinkToSource(AudioSource aSource, bool playOnAwake)
        {
            source = aSource;
            source.playOnAwake = playOnAwake;

            source.clip = _clip;

            if (endTime < 0)
                endTime = _clip.length;
        }

        //Randomizes then plays.
        public void Play()
        {
            Randomize();
            source.Play();
            source.time = startTime;
        }

        //Used for external audio source that will destruct upon completion.
        public void Play(AudioSourceEvents aSource)
        {
            Randomize();
            aSource.BeginPlay();
            aSource.Source.time = startTime;
            aSource.EndTime = endTime;
        }

        //Randomizes volume and pitch.
        private void Randomize()
        {
            source.volume = GetRandomVolume();
            source.pitch = GetRandomPitch();
        }

        private float GetRandomVolume()
            => Random.Range(_volumeRange.x, _volumeRange.y);

        private float GetRandomPitch()
            => Random.Range(_pitchRange.x, _pitchRange.y);
    }

}

