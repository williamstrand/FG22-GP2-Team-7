using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public GameObject[] spikes;
    public float raiseHeight = 1.0f;
    public float raiseSpeed = 1.0f;
    public float retractSpeed = 1.0f;
    public float retractInterval = 1.0f;
    public bool retractSpikes = true;
    public bool repeat = false;

    private Vector3[] originalPositions;
    private Vector3[] raisedPositions;
    private bool raised = false;

    void Start()
    {
        originalPositions = new Vector3[spikes.Length];
        raisedPositions = new Vector3[spikes.Length];
        for (int i = 0; i < spikes.Length; i++)
        {
            originalPositions[i] = spikes[i].transform.position;
            raisedPositions[i] = originalPositions[i] + Vector3.up * raiseHeight;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RaiseSpikes();
        }
    }

    void RaiseSpikes()
    {
        raised = true;
        for (int i = 0; i < spikes.Length; i++)
        {
            spikes[i].transform.position = Vector3.Lerp(spikes[i].transform.position, raisedPositions[i], Time.deltaTime * raiseSpeed);
        }
        if (retractSpikes)
        {
            Invoke("RetractSpikes", retractInterval);
        }
    }

    void RetractSpikes()
    {
        raised = false;
        for (int i = 0; i < spikes.Length; i++)
        {
            spikes[i].transform.position = Vector3.Lerp(spikes[i].transform.position, originalPositions[i], Time.deltaTime * retractSpeed);
        }
        if (repeat)
        {
            Invoke("RaiseSpikes", retractInterval);
        }
    }

    void Update()
    {
        if (raised)
        {
            for (int i = 0; i < spikes.Length; i++)
            {
                spikes[i].transform.position = Vector3.Lerp(spikes[i].transform.position, raisedPositions[i], Time.deltaTime * raiseSpeed);
            }
        }
        else
        {
            for (int i = 0; i < spikes.Length; i++)
            {
                spikes[i].transform.position = Vector3.Lerp(spikes[i].transform.position, originalPositions[i], Time.deltaTime * retractSpeed);
            }
        }
    }
}
