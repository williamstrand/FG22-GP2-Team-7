using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float waterLevel;
    public float floatThreshold;
    public float waterDensity = 0.125f;
    public float downForce = 4.0f;
    public float waveHeight = 0.1f;
    public float waveFrequency = 0.5f;
    public float damping = 0.1f;

    private float forceFactor;
    private Vector3 floatForce;

    void Update()
    {
        forceFactor = 1.0f - ((transform.position.y - waterLevel) / floatThreshold) + Mathf.Sin(Time.time * waveFrequency) * waveHeight;

        if (forceFactor > 0f)
        {
            floatForce = -Physics.gravity * (forceFactor - GetComponent<Rigidbody>().velocity.y * waterDensity);
            floatForce += new Vector3(0.0f, -downForce, 0.0f);
            floatForce = floatForce * GetComponent<Rigidbody>().mass;
            GetComponent<Rigidbody>().AddForceAtPosition(floatForce, transform.position);
            GetComponent<Rigidbody>().velocity *= 1.0f - damping * Time.deltaTime;
        }
    }
}