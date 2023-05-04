using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    internal class AudioOptionManager : MonoBehaviour
    {
        #region Fields

        internal static AudioOptionManager Instance { get; private set; }

        [SerializeField] private AudioMixer _audioMixer;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        public void SetMasterVolume(float volume)
        {
            _audioMixer.SetFloat("Master", volume > 0 ? Mathf.Log(volume) *20f : -80f);
        }

        public void SetMusicVolume(float volume)
        {
            _audioMixer.SetFloat("Master", volume > 0 ? Mathf.Log(volume) * 20f : -80f);
        }

        public void SetEffectsVolume(float volume)
        {
            _audioMixer.SetFloat("Master", volume > 0 ? Mathf.Log(volume) * 20f : -80f);
            
            //Play an exemplary SFX to give the play an auditory volume feedback
            AudioManager.Instance.PlaySoundEffectOnce(SFX.ButtonClick);
        }

        #endregion
    }
}
