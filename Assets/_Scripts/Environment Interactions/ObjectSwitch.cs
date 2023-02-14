using UnityEngine;

public class ObjectSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    [SerializeField] private TriggerController _trigger;
    
    /// <summary>
    /// Toggle showing/hiding objects
    /// </summary>
    [SerializeField] private bool revealWhenPressed = true;
    
    [Header("Object Fading")]
    [SerializeField] private bool fadeInOut = false;
    [SerializeField] private float fadeDuration = 1f;

    private MeshRenderer _meshRenderer;
    private Color _startColor;
    private Color _endColor;
    private float _timeStartedLerping;

    private void Start()
    {
        _meshRenderer = _object.GetComponent<MeshRenderer>();
        _startColor = _meshRenderer.material.color;
        _endColor = revealWhenPressed ? new Color(_startColor.r, _startColor.g, _startColor.b, 1f) : new Color(_startColor.r, _startColor.g, _startColor.b, 0f);
    }

    private void Update()
    {
        SwitchObject();
    }

    private void SwitchObject()
    {
        if (_trigger.isPressed)
        {
            if (fadeInOut)
            {
                _timeStartedLerping = Time.time;
                FadeInOutObject();
            }
            else
            {
                _object.SetActive(revealWhenPressed);
            }
        }
        else
        {
            if (fadeInOut)
            {
                _timeStartedLerping = Time.time;
                FadeInOutObject();
            }
            else
            {
                _object.SetActive(!revealWhenPressed);
            }
        }
    }

    private void FadeInOutObject()
    {
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / fadeDuration;

        

        if (percentageComplete >= 1)
        {
            _object.SetActive(revealWhenPressed);
            return;
        }
        _meshRenderer.material.color = Color.Lerp(_startColor, _endColor, percentageComplete);
    }
}