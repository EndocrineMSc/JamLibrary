using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Number (int) or text pop up, destroys itself after a set time.
/// </summary>
public class VFXPopUp : MonoBehaviour
{
    #region Fields and Properties

    private readonly float _tweenDuration = 2f;
    private readonly float _defaultFontSize = 45;
    private TextMeshPro _textMesh;

    #endregion

    #region Functions

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
    }

    //for damage numbers, scales color and/or size with damage amount
    public void Setup(int amount, string hexColor = "#FF0A01")
    {
        _textMesh.text = amount.ToString();
        _textMesh.fontSize = _defaultFontSize;

        var sizeScale = 0.01f;
        var colorScale = Mathf.Clamp01(amount / 10000f);

        ColorUtility.TryParseHtmlString(hexColor, out Color basicColor);
        _textMesh.color = Color.Lerp(_textMesh.color, basicColor, colorScale);

        transform.DOMove(transform.position + Vector3.one, _tweenDuration);
        transform.DOScale(sizeScale * Vector3.one, _tweenDuration);
        _textMesh.DOFade(0, _tweenDuration);
        StartCoroutine(DestroyAfterDelay());
    }

    //for text pop ups
    public void Setup(string barkText, string hexColor = "#FF0A01")
    {
        _textMesh.text = barkText;

        ColorUtility.TryParseHtmlString(hexColor, out Color basicColor);
        _textMesh.color = basicColor;

        transform.DOJump(transform.position + new Vector3(1, 0.5f, 0), 1, 1, _tweenDuration);
        _textMesh.DOFade(0, _tweenDuration);
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(_tweenDuration * 1.2f);
        Destroy(gameObject);
    }

    #endregion
}

