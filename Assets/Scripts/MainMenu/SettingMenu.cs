using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Settings Menu containing volume settings.
/// Attach to a SettingsMenuCanvas and add a return button (to main menu) as child.
/// </summary>
public class SettingMenu : MonoBehaviour
{
    #region Fields and Properties

    private Canvas _settingsCanvas;
    private Button _backButton;

    [SerializeField] private Slider _masterVolume;
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Slider _sfxVolume;

    #endregion

    #region Methods

    private void Awake()
    {
        _settingsCanvas = GetComponent<Canvas>();
        _settingsCanvas.enabled = false;
    }

    private void OnEnable()
    {
        MainMenu.OnSettingsMenuOpened += OnSettingsOpened;
        MainMenu.OnMainMenuOpened += OnOtherMenuOpened;
        MainMenu.OnCreditsMenuOpened += OnOtherMenuOpened;
    }

    private void Start()
    {
        _backButton = GetComponentInChildren<Button>();

        _backButton.onClick.AddListener(OpenMainMenu);
        
        _masterVolume.onValueChanged.AddListener(SetMasterVolume);
        _musicVolume.onValueChanged.AddListener(SetMusicVolume);
        _sfxVolume.onValueChanged.AddListener(SetSoundVolume);       
    }

    private void OnDisable()
    {
        MainMenu.OnSettingsMenuOpened -= OnSettingsOpened;
        MainMenu.OnMainMenuOpened -= OnOtherMenuOpened;
        MainMenu.OnCreditsMenuOpened -= OnOtherMenuOpened;
    }

    private void OnSettingsOpened()
    {
        _settingsCanvas.enabled = true;

        _masterVolume.value = AudioManager.Instance.MasterVolume;
        _musicVolume.value = AudioManager.Instance.MusicVolume;
        _sfxVolume.value = AudioManager.Instance.SoundVolume;
    }

    private void OnOtherMenuOpened()
    {
        _settingsCanvas.enabled = false;
    }

    private void OpenMainMenu()
    {
        MainMenu.RaiseMainMenuOpened();
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

