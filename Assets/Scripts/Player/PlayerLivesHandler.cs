using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Player Life UI and functionality for games with a limited amount of lives.
/// Set an image for each live in the inspector or adapt script.
/// 3 lives: X X X; 2 lives : X X; etc.
/// </summary>
public class PlayerLivesHandler : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] private List<Image> _activeLifeImages;
    private List<Image> _inactiveLifeImages = new();
    private int _maxLives;

    public int Lives { get; private set; } = 3;
    public static event Action OnPlayerDeath;

    #endregion

    #region Methods

    private void Awake()
    {
        _maxLives = _activeLifeImages.Count;
    }

    public void LoseLife()
    {
        if (Lives >= 0)
        {
            Lives--;
            DisableLifeImage();
        }
        else
        {
            OnPlayerDeath?.Invoke();
        }
    }

    public void GainLife(int amount = 1)
    {
        var resultLives = Lives + amount;

        if (resultLives <= _maxLives)
        {
            Lives = resultLives;

            for (int i = 0; i < amount; i++)
                EnableLifeImage();
        }
        else if (Lives < _maxLives)
        {
            var difference = _maxLives - Lives;
            Lives = _maxLives;

            for (int i = 0; i < difference; i++)
                EnableLifeImage();
        }
    }

    private void DisableLifeImage()
    {
        _activeLifeImages[_activeLifeImages.Count - 1].enabled = false;
        _inactiveLifeImages.Add(_activeLifeImages[_activeLifeImages.Count - 1]);
        _activeLifeImages.RemoveAt(_activeLifeImages.Count - 1);
    }

    private void EnableLifeImage()
    {
        _activeLifeImages.Add(_inactiveLifeImages[0]);
        _inactiveLifeImages[0].enabled = true;
        _inactiveLifeImages.RemoveAt(0);
    }

    #endregion
}
