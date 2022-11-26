using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using GameName;
using GameName.Audio;

namespace GameName.Buttons
{

    //this class is meant to provide simple functions for
    //menu Button functionalities, can be extended for further
    //Button functionalities
    //propably most useful as a component of the GameManager
    //otherwise add empty gameobject to scene (add singleton
    //and dontdestroyonload if needed
    public class ButtonManager : MonoBehaviour
    {
        #region Fields

        private GameManager _instance;
        private AudioManager _audioManager;

        #endregion

        #region Public Functions

        public void StartGame()
        {
            _audioManager.PlaySoundEffect(SFX.ButtonClick);
            _instance.SwitchState(GameState.Starting);
        }

        public void QuitGame()
        {
            _audioManager.PlaySoundEffect(SFX.ButtonClick);
            _instance.SwitchState(GameState.Quit);

        }

        public void GoToCredits()
        {
            _audioManager.PlaySoundEffect(SFX.ButtonClick);
            _instance.SwitchState(GameState.Credits);

        }

        public void GoToHighscores()
        {
            _audioManager.PlaySoundEffect(SFX.ButtonClick);
            _instance.SwitchState(GameState.HighscoreMenu);
        }

        public void StartNewGame()
        {
            _audioManager.PlaySoundEffect(SFX.ButtonClick);
            _instance.SwitchState(GameState.NewGame);
        }

        public void GoToSettings()
        {
            _audioManager.PlaySoundEffect(SFX.ButtonClick);
            _instance.SwitchState(GameState.Settings);
        }


        //meant to be called by "back" buttons of the different
        //screens in the menu - not to be called to start a new
        //game before "StartNewGame"
        public void GoToMainMenu()
        {
            _audioManager.PlaySoundEffect(SFX.ButtonClick);
            _instance.SwitchState(GameState.MainMenu);
        }

        #endregion

        #region Private Functions

        // Start is called before the first frame update
        private void Start()
        {
            _instance = GameManager.Instance;
            _audioManager = AudioManager.Instance;
        }

        #endregion
    }
}
