using UnityEngine;

public class TrapController : MonoBehaviour
{
[SerializeField] Transform[] trapObjects;
[SerializeField] float raiseAmount = 1f;
[SerializeField] float retractAmount = -1f;
[SerializeField] float moveAmount = 1f;
[SerializeField] bool moveHorizontally = false;
[SerializeField] bool raiseOnEnter = true;
[SerializeField] bool retractOnExit = true;
[SerializeField] float smoothTime = 0.3f;
[SerializeField] TriggerController triggerController;



private Vector3[] initialPositions;
private Vector3[] targetPositions;
private Vector3[] velocities;

private bool isRaised = false;

private void Start()
{
    initialPositions = new Vector3[trapObjects.Length];
    targetPositions = new Vector3[trapObjects.Length];
    velocities = new Vector3[trapObjects.Length];

    for (int i = 0; i < trapObjects.Length; i++)
    {
        initialPositions[i] = trapObjects[i].position;
        targetPositions[i] = initialPositions[i];
    }
}

private void Update()
{
    if (triggerController.isPressed && raiseOnEnter && !isRaised)
    {
        for (int i = 0; i < trapObjects.Length; i++)
        {
            targetPositions[i] = initialPositions[i] + Vector3.up * raiseAmount;
            if (moveHorizontally)
            {
                targetPositions[i] += Vector3.right * moveAmount;
            }
        }
        isRaised = true;
    }
    else if (!triggerController.isPressed && retractOnExit && isRaised)
    {
        for (int i = 0; i < trapObjects.Length; i++)
        {
            targetPositions[i] = initialPositions[i] + Vector3.up * retractAmount;
            if (moveHorizontally)
            {
                targetPositions[i] += Vector3.right * moveAmount;
            }
        }
        isRaised = false;
    }

    for (int i = 0; i < trapObjects.Length; i++)
    {
        trapObjects[i].position = Vector3.SmoothDamp(trapObjects[i].position, targetPositions[i], ref velocities[i], smoothTime);
    }
}

public void ResetTrapPosition()
{
    for (int i = 0; i < trapObjects.Length; i++)
    {
        trapObjects[i].position = initialPositions[i];
        targetPositions[i] = initialPositions[i];
    }
    isRaised = false;
}

}