using UnityEngine;

/// <summary>
/// Let's you pause the game.
/// Freezes time and therefore all time-based movement and animations.
/// Will not freeze all input, so you can still press buttons and move the mouse.
/// If you have movement by inputs, that is not time-based, put a check for GameIsPaused 
/// in the movement script.
/// </summary>
public class PauseControl
{
    #region Fields and Properties

    public static bool GameIsPaused { get; private set; }

    #endregion

    #region Functions

    public static void PauseGame()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    #endregion
}
