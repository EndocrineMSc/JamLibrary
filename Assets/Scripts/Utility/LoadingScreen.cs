using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads the next Scene after a delay, referencing the LoadHelper class
/// </summary>
public class LoadingScreen : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitForLoadingScreen());
    }

    private IEnumerator WaitForLoadingScreen()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(LoadHelper.SceneToBeLoaded.ToString());
    }
}
