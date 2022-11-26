using System;
using System.Collections.Generic;
using System.Collections;
using EnumCollection;
using GameName.Audio;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Converters;

namespace GameName
{
    //The GameManager is a Singleton that is always present,
    //even when loading new Scenes
    //Just put an empty GameObject in the Scene, name it GameManager
    //and attach this script as a component
    public class GameManager : MonoBehaviour
    {
        #region Fields

        public static GameManager Instance;

        //Drag and Drop the respective Canvases to these
        [SerializeField] private GameObject _menuScreen;
        [SerializeField] private GameObject _creditsScreen;
        [SerializeField] private GameObject _settingsScreen;
        [SerializeField] private GameObject _highscoreScreen;        

        #endregion

        #region Properties

        //allows for getting the GameState by other scripts
        //setting only over SwitchState outside of GameManager
        private GameState _state;
        public GameState State
        {
            get { return _state; }
            private set { _state = value; }
        }

        #endregion

        #region Public Functions

        //State Machine using the GameState enum of EnumCollections
        //EnumCollections is a separate Script that needs to be present
        //in the project
        public void SwitchState(GameState state)
        {
            Instance._state = state;
            switch (state)
            {
                case (GameState.MainMenu):
                    Scene _scene = SceneManager.GetActiveScene();
                    string _sceneName = _scene.name;

                    if (_sceneName != "MainMenu")
                    {
                        SceneManager.LoadSceneAsync("MainMenu");
                    }

                    CloseAllCanvases();
                    Instance._menuScreen.SetActive(true);
                    AudioManager.Instance.FadeGameTrack(Track.MainMenu);
                    break;

                case (GameState.Credits):
                    CloseAllCanvases();
                    Instance._creditsScreen.SetActive(true);
                    break; 

                case (GameState.Settings):
                    CloseAllCanvases();
                    Instance._settingsScreen.SetActive(true);
                    break;   

                case (GameState.HighscoreMenu):
                    CloseAllCanvases();
                    Instance._highscoreScreen.SetActive(true);
                    break; 

                case (GameState.Starting):
                    CloseAllCanvases();
                    //PlayGameTrack is a Method in the AudioManager
                    //It plays the Track that is referenced by using a
                    //Track enum in EnumCollection
                    SceneManager.LoadSceneAsync("LevelOne");
                    AudioManager.Instance.FadeGameTrack(Track.MainMenu);
                    AudioManager.Instance.FadeGameTrack(Track.GameTrackOne);
                    break;

                case (GameState.GameOver):
                    //do end of game stuff here
                    break;

                case (GameState.NewGame):
                    //do reset game stuff here, in case player restarts in-game
                    break;

                case (GameState.Quit):
#if UNITY_EDITOR
                    EditorApplication.ExitPlaymode();
#endif  
                    Application.Quit();
                    break;
            }
        }

        #endregion

        #region Private Functions

        //closes all canvases, meant for state-changes in SwitchState()
        private void CloseAllCanvases()
        {
            Instance._menuScreen.SetActive(false);
            Instance._creditsScreen.SetActive(false);
            Instance._settingsScreen.SetActive(false);
            Instance._highscoreScreen.SetActive(false);
        }

        //Awake, so that it runs before the Start Functions of other
        //scripts that need to reference "Instance" in Start
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            DontDestroyOnLoad(this);                 
        }

        private void Start()
        {
            //starts the game in the main menu
            Instance.SwitchState(GameState.MainMenu);
        }

        #endregion

    }
}