using Audio;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Utility
{
    internal class GameManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static GameManager Instance { get; private set; }   
        internal GameState State;

        #endregion

        #region Functions

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
        }

        private void Start()
        {
            SwitchState(GameState.MainMenu);
        }

        public void SwitchState(GameState state)
        {
            Instance.State = state;

            switch (state)
            {
                case (GameState.MainMenu):
                    if (SceneManager.GetActiveScene().name != "MainMenu")
                        SceneManager.LoadSceneAsync("MainMenu");

                    AudioManager.Instance.FadeGameTrack(GameTrack.MainMenu, Fade.In);
                    break;

                case (GameState.NewGame):
                    SceneManager.LoadSceneAsync("LevelOne");
                    AudioManager.Instance.FadeGameTrack(GameTrack.MainMenu, Fade.Out);
                    AudioManager.Instance.FadeGameTrack(GameTrack.GameTrackOne, Fade.In);
                    break;

                case (GameState.GameOver):
                    break;

                case (GameState.Victory):
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
    }

    internal enum GameState
    {
        MainMenu,
        NewGame,
        GameOver,
        Victory,
        Quit
    }
}