using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Audio;

namespace Utility
{
    internal class MenuManager : MonoBehaviour
    {
        #region Fields and Properties

        private GameManager _gameManager;
        
        private List<Canvas> _canvasList;
        private Canvas _mainMenuScreen;
        private Canvas _creditsScreen;
        private Canvas _settingsScreen;
        private Canvas _highscoreScreen;
        private Canvas _howToPlayScreen;

        private const string MAINMENUSCREEN_NAME = "MainMenuScreen";
        private const string CREDITSSCREEN_NAME = "CreditsScreen";
        private const string SETTINGSSCREEN_NAME = "SettingsScreen";
        private const string HIGHSCORESCREEN_NAME = "HighscoreScreen";
        private const string HOWTOPLAYSCREEN_NAME = "HowToPlayScreen";

        private const string BACKBUTTON_NAME = "BackButton";
        private const string STARTBUTTON_NAME = "StartButton";
        private const string CREDITSBUTTON_NAME = "CreditsButton";
        private const string HIGHSCOREBUTTON_NAME = "HighScoreButton";
        private const string HOWTOPLAYBUTTON_NAME = "HowToPlayButton";
        private const string SETTINGSBUTTON_NAME = "SettingsButton";

        private const string MASTERSLIDER_NAME = "MasterSlider";
        private const string MUSICSLIDER_NAME = "MusicSlider";
        private const string SOUNDEFFECTSSLIDER_NAME = "SoundEffectsSlider";

        #endregion

        #region Functions

        void Start()
        {
            SetReferences();
            SetCanvases();
            SwitchCanvas(MenuState.MainMenu);
            SetButtonFunctions();
            SetSliderFunctions();
        }

        private void SetReferences()
        {
            _gameManager = GameManager.Instance;           
        }

        private void SetCanvases()
        {
            _canvasList = GameObject.FindObjectsOfType<Canvas>().ToList();

            foreach (Canvas canvas in _canvasList)
            {
                switch (canvas.gameObject.name)
                {
                    case MAINMENUSCREEN_NAME:
                        _mainMenuScreen = canvas; break;
                    case CREDITSSCREEN_NAME:
                        _creditsScreen = canvas; break;
                    case SETTINGSSCREEN_NAME:
                        _settingsScreen = canvas; break;
                    case HIGHSCORESCREEN_NAME:
                        _highscoreScreen = canvas; break;
                    case HOWTOPLAYSCREEN_NAME:
                        _howToPlayScreen = canvas; break;
                }
            }
        }

        private void SwitchCanvas(MenuState state)
        {
            CloseAllCanvases();

            switch (state)
            {
                case MenuState.MainMenu:
                    if (_mainMenuScreen != null)
                    {
                        _mainMenuScreen.enabled = true;
                        _mainMenuScreen.GetComponentInChildren<Image>().DOFade(1, 0.5f);
                    }
                    break;
                case MenuState.Settings:
                    if (_settingsScreen != null)
                    {
                        _settingsScreen.enabled = true;
                        _settingsScreen.GetComponentInChildren<Image>().DOFade(1, 0.5f);
                    }
                    break;
                case MenuState.Credits:
                    if (_creditsScreen != null)
                    {
                        _creditsScreen.enabled = true;
                        _creditsScreen.GetComponentInChildren<Image>().DOFade(1, 0.5f);
                    }
                    break;
                case MenuState.Highscore:
                    if (_highscoreScreen != null)
                    {
                        _highscoreScreen.enabled = true;
                        _highscoreScreen.GetComponentInChildren<Image>().DOFade(1, 0.5f);
                    }
                    break;
                case MenuState.HowToPlay:
                    if (_howToPlayScreen != null)
                    {
                        _howToPlayScreen.enabled = true;
                        _howToPlayScreen.GetComponentInChildren<Image>().DOFade(1, 0.5f);
                    }
                    break;
            }
        }

        private void SetButtonFunctions()
        {
            List<Button> buttons = GameObject.FindObjectsOfType<Button>().ToList();

            foreach (Button button in buttons)
            {
                switch (button.name)
                {
                    case STARTBUTTON_NAME:
                        button.onClick.AddListener(OnStartButtonClick);
                        break;
                    case CREDITSBUTTON_NAME:
                        button.onClick.AddListener(OnCreditsButtonClick);
                        break;
                    case HIGHSCOREBUTTON_NAME:
                        button.onClick.AddListener(OnHighscoreButtonClick);
                        break;
                    case HOWTOPLAYBUTTON_NAME:
                        button.onClick.AddListener(OnHowToPlayButtonClick);
                        break;
                    case SETTINGSBUTTON_NAME:
                        button.onClick.AddListener(OnSettingsButtonClick);
                        break;
                    case BACKBUTTON_NAME:
                        button.onClick.AddListener(OnBackButtonClick);
                        break;
                }
            }
        }

        private void SetSliderFunctions()
        {
            List<Slider> sliders = GameObject.FindObjectsOfType<Slider>().ToList();

            foreach (Slider slider in sliders)
            {
                switch (slider.name)
                {
                    case MASTERSLIDER_NAME:
                        slider.onValueChanged.AddListener(AudioOptionManager.Instance.SetMasterVolume);
                        break;
                    case MUSICSLIDER_NAME:
                        slider.onValueChanged.AddListener(AudioOptionManager.Instance.SetMusicVolume);
                        break;
                    case SOUNDEFFECTSSLIDER_NAME:
                        slider.onValueChanged.AddListener(AudioOptionManager.Instance.SetEffectsVolume);
                        break;
                }
                
            }

        }


        private void CloseAllCanvases()
        {
            foreach (Canvas canvas in _canvasList) 
            {
                if (canvas != null && canvas.enabled)
                {
                    Image canvasBackground = canvas.GetComponentInChildren<Image>();
                    canvasBackground.DOFade(0, 0.5f);
                    canvas.enabled = false;
                }
            }
        }

        //Button Functions
        public void OnBackButtonClick()
        {
            SwitchCanvas(MenuState.MainMenu);
        }

        public void OnStartButtonClick()
        {
            _gameManager.SwitchState(GameState.NewGame);
        }

        public void OnSettingsButtonClick()
        {
            SwitchCanvas(MenuState.Settings);
        }

        public void OnCreditsButtonClick()
        {
            SwitchCanvas(MenuState.Credits);
        }

        public void OnHighscoreButtonClick()
        {
            SwitchCanvas(MenuState.Highscore);
        }

        public void OnHowToPlayButtonClick()
        {
            SwitchCanvas(MenuState.HowToPlay);
        }

        #endregion
    }

    internal enum MenuState 
    { 
        MainMenu, 
        Credits, 
        Settings, 
        Highscore,
        HowToPlay
    }
}
