using UnityEngine;

/// <summary>
/// Attack to an object that needs VFX Pop ups.
/// Supports number (int) and text pop ups (barks, damage numbers, etc.)
/// </summary>
public class VFXPopUpSpawner : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] private VFXPopUp _popUpPrefab;

    #endregion

    #region Functions

    public void SpawnPopUp(int damageAmount, string hexColor = "#FF0A01")
    {
        Transform spawnTransform = gameObject.transform;
        float xOffset = spawnTransform.position.x + UnityEngine.Random.Range(-0.1f, 0.1f);
        float yOffset = spawnTransform.position.y + UnityEngine.Random.Range(-0.1f, 0.1f);

        Vector3 spawnPosition = new(xOffset, yOffset, spawnTransform.position.z);
        VFXPopUp tempPopUp = Instantiate(_popUpPrefab, spawnPosition, Quaternion.identity);
        tempPopUp.Setup(damageAmount, hexColor);
    }

    public void SpawnPopUp(string bark, string hexColor = "#FF0A01")
    {
        Transform spawnTransform = transform.GetChild(0).gameObject.transform;
        float xOffset = spawnTransform.position.x + UnityEngine.Random.Range(-0.1f, 0.1f);
        float yOffset = spawnTransform.position.y + UnityEngine.Random.Range(-0.1f, 0.1f);

        Vector3 spawnPosition = new(xOffset, yOffset, spawnTransform.position.z);
        VFXPopUp tempPopUp = Instantiate(_popUpPrefab, spawnPosition, Quaternion.identity);
        tempPopUp.Setup(bark, hexColor);
    }

    #endregion
}

