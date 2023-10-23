using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Opens a screen to display the credits.
/// Attach to a CreditsCanvas and add a return button (to main menu) as child.
/// </summary>
public class CreditsMenu : MonoBehaviour
{
    #region Fields and Properties

    private Canvas _creditsCanvas;
    private Button _backButton;

    #endregion

    #region Functions

    private void Awake()
    {
        _creditsCanvas = GetComponent<Canvas>();
        _creditsCanvas.enabled = false;
    }

    private void OnEnable()
    {
        MainMenu.OnCreditsMenuOpened += OnCreditsOpened;
        MainMenu.OnSettingsMenuOpened += OnOtherMenuOpened;
        MainMenu.OnMainMenuOpened += OnOtherMenuOpened;
    }

    private void Start()
    {
        _backButton = GetComponentInChildren<Button>();
        _backButton.onClick.AddListener(OpenMainMenu);
    }

    private void OnDisable()
    {
        MainMenu.OnCreditsMenuOpened -= OnCreditsOpened;
        MainMenu.OnSettingsMenuOpened -= OnOtherMenuOpened;
        MainMenu.OnMainMenuOpened -= OnOtherMenuOpened;
    }

    private void OnCreditsOpened()
    {
        _creditsCanvas.enabled = true;
    }

    private void OnOtherMenuOpened()
    {
        _creditsCanvas.enabled = false;
    }

    private void OpenMainMenu()
    {
        PlayButtonClick();
        MainMenu.RaiseMainMenuOpened();
    }

    private void PlayButtonClick()
    {
        //AudioManager.Instance.PlaySoundEffectOnce(SFX._0001_ButtonClick);
    }

    #endregion
}
