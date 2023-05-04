using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace Utility
{
    internal class PauseControl : MonoBehaviour
    {
        #region Fields and Properties

        internal static PauseControl Instance { get; private set; }
        internal bool GameIsPaused { get; private set; }

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                GameIsPaused = false;
            }
            else
                Destroy(gameObject);
        }

        void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ResumeGame();
        }

        internal void PauseGame()
        {
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

        internal void ResumeGame()
        {
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        #endregion
    }
}
