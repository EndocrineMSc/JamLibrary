using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A simple pause menu for volume control and pausing the game.
/// Put into every scene you want to have the menu in.
/// Default button set to "P", because of WebGL "Esc" incompability.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    #region Fields and Properties

    private Canvas _settingsCanvas;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;

    #endregion

    #region Methods

    private void Start()
    {
        _settingsCanvas = GetComponent<Canvas>();
        _settingsCanvas.enabled = false;
        _masterSlider.onValueChanged.AddListener(SetMasterVolume);
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _soundSlider.onValueChanged.AddListener(SetSoundVolume);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (PauseControl.GameIsPaused)
            {
                PauseControl.ResumeGame();
                _settingsCanvas.enabled = false;
            }
            else
            {
                PauseControl.PauseGame();
                _settingsCanvas.enabled = true;

                _masterSlider.value = AudioManager.Instance.MasterVolume;
                _musicSlider.value = AudioManager.Instance.MusicVolume;
                _soundSlider.value = AudioManager.Instance.SoundVolume;
            }
        }
    }

    private void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
    }

    private void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    private void SetSoundVolume(float value)
    {
        AudioManager.Instance.SetSoundVolume(value);
    }

    public void ResumeGameButton()
    {
        PauseControl.ResumeGame();
        _settingsCanvas.enabled = false;
    }

    #endregion
}
