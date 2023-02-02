using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    public bool isPressed;
    [SerializeField] bool _isOnOff;
    //[SerializeField] Transform _button, _buttonDown;
    //private Vector3 _buttonUp;


    private void Start()
    {
        //_buttonUp = _button.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isOnOff)
            {
                if (isPressed)
                {
                    //_button.position = _buttonUp;
                    isPressed = false;
                }
                else
                {
                    //_button.position = _buttonDown.position;
                    isPressed = true;
                }
            }
            else
            {
                if (!isPressed)
                {
                    //_button.position = _buttonDown.position;
                    isPressed = true;
                }
            }
        }
    }
}