using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 1.0f;
    public GameObject triggerObject;
    public bool shouldReturn = false;
    private TriggerController triggerController;
    private Vector3 currentTarget;
    private float startTime;
    private float journeyLength;

    private void Start()
    {
        if (triggerObject != null)
        {
            triggerController = triggerObject.GetComponent<TriggerController>();
            if (triggerController == null)
            {
                Debug.LogError("The trigger object does not have a TriggerController script attached.");
            }
        }
        else
        {
            Debug.LogError("The trigger object has not been set in the ElevatorMover script.");
        }

        currentTarget = endPoint.position;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPoint.position, endPoint.position);
    }

    private void Update()
    {
        if (triggerController != null && triggerController.isPressed)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startPoint.position, currentTarget, fracJourney);

       if (transform.position == startPoint.position)
        {
            currentTarget = endPoint.position;
            startTime = Time.time;
            journeyLength = Vector3.Distance(startPoint.position, endPoint.position);
        }
    }
}
