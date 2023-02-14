using System.Collections;
using UnityEngine;
public class TriggerController : MonoBehaviour
{
    /// <summary>
    /// Shows if the trigger is activated or not.
    /// </summary>
    public bool isPressed;
    /// <summary>
    /// If set to true, toggle mode is activated.
    /// </summary>
    [SerializeField] bool _isOnOff;

    [SerializeField] bool _useAnimation;
    [SerializeField] float _animationDuration = 1f;
    [SerializeField] Transform _triggerTransform, _triggerDownTransform;
    private Vector3 _triggerUpPosition;

    private void Start()
    {
        //_buttonUp = _button.position;
        _triggerUpPosition = _triggerTransform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isOnOff)
            {
                if (isPressed)
                {
                    isPressed = false;
                }
                else
                {
                    PullLever();
                }
            }
            else
            {
                if (!isPressed)
                {
                    PullLever();
                }
            }
        }
    }

    public void PullLever()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isPressed = true;

            if (_useAnimation)
            {
                StartCoroutine(MoveLever(_triggerDownTransform.position));
            }
            else
            {
                _triggerTransform.position = _triggerDownTransform.position;
            }
        }
    }

    private IEnumerator MoveLever(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = _triggerTransform.position;

        while (elapsedTime < _animationDuration)
        {
            _triggerTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / _animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _triggerTransform.position = targetPosition;
    }

}