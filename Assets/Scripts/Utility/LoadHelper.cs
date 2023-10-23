using UnityEngine.SceneManagement;

/// <summary>
/// Holds data about scenes to be loaded and artifical loading time.
/// Makes it easy to load with a loading screen.
/// </summary>
public class LoadHelper
{
    public static SceneName SceneToBeLoaded { get; private set; } = SceneName.LevelOne;
    public static float LoadDuration { get; private set; } = 1f;

    public static void LoadSceneWithLoadingScreen(SceneName sceneName)
    {
        SceneToBeLoaded = sceneName;
        SceneManager.LoadSceneAsync(SceneName.LoadingScreen.ToString());
    }
}

public enum SceneName
{
    MainMenu,
    LevelOne,
    LoadingScreen,
    GameOver,
}