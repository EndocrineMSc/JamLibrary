using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    internal class AudioManager : MonoBehaviour
    {
        #region Fields and Properties

        public static AudioManager Instance { get; private set; }

        private List<AudioSource> _soundEffects;
        private List<AudioSource> _gameTracks;

        [SerializeField] private AudioMixerGroup _SFX;
        [SerializeField] private AudioMixerGroup _music;

        private readonly string _gameTracksResourcesFolder = "GameTracks";
        private readonly string _soundEffectsResourcesFolder = "SoundEffects";

        #endregion

        #region Functions

        private void Awake()
        {

            if( Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
          
            //Builds the Lists and AudioSources to be used from the Assets/Resources Folder
            _gameTracks = BuildGameTrackListAndAttachAudioSources();
            _soundEffects = BuildSoundEffectListAndAttackAudioSources();
        }

        private List<AudioSource> BuildGameTrackListAndAttachAudioSources()
        {
            AudioClip[] _gameTrackArray = Resources.LoadAll<AudioClip>(_gameTracksResourcesFolder);
            List<AudioSource> _tempList = new();

            foreach (AudioClip _clip in _gameTrackArray)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();

                audioSource.clip = _clip;
                audioSource.loop = true;
                audioSource.volume = 0;
                audioSource.playOnAwake = true;
                audioSource.outputAudioMixerGroup = _music;

                _tempList.Add(audioSource);
            }
            return _tempList;
        }

        private List<AudioSource> BuildSoundEffectListAndAttackAudioSources()
        {
            AudioClip[] _soundEffectArray = Resources.LoadAll<AudioClip>(_soundEffectsResourcesFolder);
            List<AudioSource> _tempList = new();

            foreach (AudioClip _clip in _soundEffectArray)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();

                audioSource.clip = _clip;
                audioSource.outputAudioMixerGroup = _SFX;
                audioSource.playOnAwake = false;

                _tempList.Add(audioSource);
            }
            return _tempList;
        }

        public void FadeGameTrack(GameTrack track, Fade fade)
        {
            AudioSource audioSource = _gameTracks[(int)track];   

            if (fade == Fade.In)
            {
                StartCoroutine(StartFade(audioSource, 3f, 1f));
            }
            else if (fade == Fade.Out) 
            {
                StartCoroutine(StartFade(audioSource, 3f, 0f));
            }
        }

        //Plays a Sound Effect according to the enum index if it isn't playing already
        public void PlaySoundEffectOnce(SFX sfx)
        {
            AudioSource audioSource = _soundEffects[(int)sfx];

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        //Plays a Sound Effect according to the enum index
        public void PlaySoundEffectUnlimitedTimes(SFX sfx)
        {
            AudioSource audioSource = _soundEffects[(int)sfx];
            audioSource.Play();
        }

        private IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float startVolume = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
                yield return null;
            }
            audioSource.volume = targetVolume;
        }

        #endregion
    }

    internal enum Fade
    {
        In,
        Out
    }

    internal enum GameTrack
    {
        MainMenu,
        GameTrackOne,
    }

    internal enum SFX
    {
        ButtonClick
    }
}