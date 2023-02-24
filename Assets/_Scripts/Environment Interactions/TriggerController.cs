using UnityEngine;


public class TriggerController : MonoBehaviour
{
    public bool isPressed;
    [SerializeField] bool _isOnOff;
    //[SerializeField] Transform _trigger, _triggerDown;
    [SerializeField] bool useOnTriggerExit = true;
    private Vector3 _triggerUp;

    private void Start()
    {
        //_buttonUp = _trigger.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Coconut"))
        {
            if (_isOnOff)
            {
                if (isPressed)
                {
                    //_trigger.position = _buttonUp;
                    isPressed = false;
                }
                else
                {
                    //_trigger.position = _triggerDown.position;
                    isPressed = true;
                }
            }
            else
            {
                if (!isPressed)
                {
                    //_trigger.position = _triggerDown.position;
                    isPressed = true;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Coconut")) return;

        if (!isPressed)
        {
            //_trigger.position = _triggerDown.position;
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (useOnTriggerExit && (other.CompareTag("Player") || other.CompareTag("Coconut")))
        {
            if (_isOnOff)
            {
                if (isPressed)
                {
                    //_trigger.position = _triggerDown.position;
                    isPressed = false;
                }
                else
                {
                    //_trigger.position = _buttonUp;
                    isPressed = true;
                }
            }
            else
            {
                if (isPressed)
                {
                    //_trigger.position = _triggerUp;
                    isPressed = false;
                }
            }
        }
    }
}