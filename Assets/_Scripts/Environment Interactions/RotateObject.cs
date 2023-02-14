
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] TriggerController _triggerController;
    [SerializeField] float _rotationAmount = 90f;
    [SerializeField] Vector3 _rotationAxis = Vector3.up;
    [SerializeField] float _speed = 1f;
    [SerializeField] bool _rotateContinuously = false;
    [SerializeField] float _damping = 10f;

    private float _totalRotation = 0f;

    private void Update()
    {
        if (_triggerController.isPressed)
        {
            if (!_rotateContinuously)
            {
                float rotationThisFrame = _rotationAmount * _speed * Time.deltaTime;
                float rotationLeft = _rotationAmount - _totalRotation;

                if (rotationLeft <= rotationThisFrame)
                {
                    transform.Rotate(_rotationAxis, rotationLeft);
                    _triggerController.isPressed = false;
                    _totalRotation = 0f;
                }
                else
                {
                    transform.Rotate(_rotationAxis, rotationThisFrame);
                    _totalRotation += rotationThisFrame;
                }
            }
            else
            {
                transform.Rotate(_rotationAxis, _rotationAmount * _speed * Time.deltaTime);
            }
        }
    }


}