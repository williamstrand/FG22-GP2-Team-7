
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public TriggerController triggerController;
    public float rotationAmount = 90f;
    public Vector3 rotationAxis = Vector3.up;
    public float speed = 1f;
    public bool rotateContinuously = false;
    public float damping = 10f;

    private float totalRotation = 0f;

    private void Update()
    {
        if (triggerController.isPressed)
        {
            if (!rotateContinuously)
            {
                float rotationThisFrame = rotationAmount * speed * Time.deltaTime;
                float rotationLeft = rotationAmount - totalRotation;

                if (rotationLeft <= rotationThisFrame)
                {
                    transform.Rotate(rotationAxis, rotationLeft);
                    triggerController.isPressed = false;
                    totalRotation = 0f;
                }
                else
                {
                    transform.Rotate(rotationAxis, rotationThisFrame);
                    totalRotation += rotationThisFrame;
                }
            }
            else
            {
                transform.Rotate(rotationAxis, rotationAmount * speed * Time.deltaTime);
            }
        }
    }


}