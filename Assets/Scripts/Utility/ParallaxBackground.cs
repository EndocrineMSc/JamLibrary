using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Attach to objects or object groups that you want to have a parallax effect.
/// The parallax effect modifier will determine the speed of the parallax effect.
/// Therefore each object or object group can have a unique parallax effect modifier.
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
    #region Fields

    [SerializeField] private Vector2 _parallaxEffectMultiplier;
    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;

    #endregion

    #region Methods

    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraPosition = _cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * _parallaxEffectMultiplier.x, deltaMovement.y * _parallaxEffectMultiplier.y);
        _lastCameraPosition = _cameraTransform.position;
    }

    #endregion
}

