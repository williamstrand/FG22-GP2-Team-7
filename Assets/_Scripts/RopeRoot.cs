using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRoot : MonoBehaviour {

    public float rigidbodyMass = 1f;
    public float colliderRadius = 0.1f;
    public float jointSpring = 0.1f;
    public float jointDamper = 5f;
    public Vector3 rotationOffset;
    public Vector3 positionOffset;

    protected List<Transform> CopySource;
    protected List<Transform> CopyDestination;
    protected static GameObject RigidBodyContainer;

    void Awake()
    {
        if(RigidBodyContainer == null)
            RigidBodyContainer = new GameObject("RopeRigidbodyContainer");

        CopySource = new List<Transform>();
        CopyDestination = new List<Transform>();

        // Add children
        AddChildren(transform);
    }

    private void AddChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            var representative = new GameObject(child.gameObject.name);
            representative.transform.parent = RigidBodyContainer.transform;
            
            //Rigidbody
            var childRigidbody = representative.gameObject.AddComponent<Rigidbody>();
            childRigidbody.useGravity = true;
            childRigidbody.isKinematic = false;
            childRigidbody.freezeRotation = true;
            childRigidbody.mass = rigidbodyMass;

            //Collider
            var collider = representative.gameObject.AddComponent<SphereCollider>();
            collider.center = Vector3.zero;
            collider.radius = colliderRadius;

            //DistanceJoint
            var joint = representative.gameObject.AddComponent<DistanceJoint3D>();
            joint.connectedRigidbody = parent;
            joint.determineDistanceOnStart = true;
            joint.spring = jointSpring;
            joint.damper = jointDamper;
            joint.determineDistanceOnStart = false;
            joint.distance = Vector3.Distance(parent.position, child.position);

            //Add copy source
            CopySource.Add(representative.transform);
            CopyDestination.Add(child);

            AddChildren(child);
        }
    }

    public void Update()
    {
        for (int i = 0; i < CopySource.Count; i++)
        {
            CopyDestination[i].position = CopySource[i].position + positionOffset;
            CopyDestination[i].rotation = CopySource[i].rotation * Quaternion.Euler(rotationOffset);
        }
    }
}
