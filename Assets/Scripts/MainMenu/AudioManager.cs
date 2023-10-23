using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

/// <summary>
/// Allows you to 
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    #region Fields and Properties

    public static AudioManager Instance { get; private set; }

    [field: SerializeField] public AudioMixer MasterMixer { get; private set; }
    [field: SerializeField] public float MasterVolume { get; private set; } = 1;
    [field: SerializeField] public float MusicVolume { get; private set; } = 1;
    [field: SerializeField] public float SoundVolume { get; private set; } = 1;
    
    private AudioSource _sfxVolumePreviewSound;

    #endregion

    #region Methods
     

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        _sfxVolumePreviewSound = GetComponent<AudioSource>();

        //Sets start volumes if the should be adjusted immediately
        SetMasterVolume(MasterVolume);
        SetMusicVolume(MusicVolume);
        SetSoundVolume(SoundVolume, false);
    }

    public void FadeInGameTrack(AudioSource audioSource)
    {
        if (!audioSource.isPlaying)
            audioSource.Play();

        audioSource.DOFade(1f, 3f);
    }

    public void FadeOutGameTrack(AudioSource audioSource, bool keepSilentlyPlaying = false)
    {
        if (!keepSilentlyPlaying)
            audioSource.DOFade(0, 3f).OnComplete(() => StopTrack(audioSource));
        else
            audioSource.DOFade(0, 3f);
    }

    private void StopTrack(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    //Plays a Sound Effect according to the enum index if it isn't playing already
    public void PlayOneShot(AudioSource audioSource)
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    //Plays a Sound Effect according to the enum index
    public void PlayUnlimited(AudioSource audioSource)
    {
        audioSource.Play();
    }

    public void SetMasterVolume(float volume)
    {
        var newVolume = GetLogCorrectedVolume(volume);
        MasterMixer.SetFloat("Master", newVolume);
        MasterVolume = newVolume;
    }

    public void SetMusicVolume(float volume)
    {
        var newVolume = GetLogCorrectedVolume(volume);
        MasterMixer.SetFloat("Music", newVolume);
        MusicVolume = newVolume;
    }

    public void SetSoundVolume(float volume, bool changedBySlider = true)
    {
        var newVolume = GetLogCorrectedVolume(volume);
        MasterMixer.SetFloat("Sound", newVolume);
        SoundVolume = newVolume;

        //Play an exemplary SFX to give the play an auditory volume feedback
        if (changedBySlider)
            PlayOneShot(_sfxVolumePreviewSound);
    }

    private float GetLogCorrectedVolume(float volume)
    {
        return volume > 0 ? Mathf.Log(volume) * 20f : -80f;
    }

    #endregion
}
