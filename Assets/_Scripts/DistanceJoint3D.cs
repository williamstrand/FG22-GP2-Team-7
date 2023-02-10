using UnityEngine;

public class DistanceJoint3D : MonoBehaviour {

    public Transform connectedRigidbody;
    public bool determineDistanceOnStart = true;
    public float distance;
    public float spring = 0.1f;
    public float damper = 5f;

    protected Rigidbody rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (determineDistanceOnStart && connectedRigidbody != null)
            distance = Vector3.Distance(rigidBody.position, connectedRigidbody.position);
    }

    void FixedUpdate()
    {

        var connection = rigidBody.position - connectedRigidbody.position;
        var distanceDiscrepancy = distance - connection.magnitude;

        rigidBody.position += distanceDiscrepancy * connection.normalized;

        var velocityTarget = connection + (rigidBody.velocity + Physics.gravity * spring);
        var projectOnConnection = Vector3.Project(velocityTarget, connection);
        rigidBody.velocity = (velocityTarget - projectOnConnection) / (1 + damper * Time.fixedDeltaTime);


    }
}