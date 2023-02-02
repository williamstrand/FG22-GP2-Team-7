using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    [SerializeField] private TriggerController _trigger;
    [SerializeField] private bool revealWhenPressed;
    [SerializeField] private bool fadeIn;
    [SerializeField] private float fadeTime = 1f;

    private MeshRenderer _meshRenderer;
    private Material _material;
    private Color _startColor;
    private Color _endColor;
    private bool _fading;

    private void Start()
    {
        _meshRenderer = _object.GetComponent<MeshRenderer>();
        _material = _meshRenderer.material;
        _startColor = _material.color;
        _endColor = new Color(_startColor.r, _startColor.g, _startColor.b, revealWhenPressed ? (fadeIn ? 1 : 0) : (fadeIn ? 0 : 1));
    }

    private void Update()
    {
        SwitchObject();
    }

    private void SwitchObject()
    {
        if (_trigger.isPressed && !_fading)
        {
            StartCoroutine(FadeObject());
        }
    }

    private IEnumerator FadeObject()
    {
        _fading = true;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            _material.color = Color.Lerp(_startColor, _endColor, elapsedTime / fadeTime);
            yield return null;
        }
        _material.color = _endColor;
        _fading = false;
    }
}