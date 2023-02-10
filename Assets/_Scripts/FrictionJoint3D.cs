using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionJoint3D : MonoBehaviour {

    [Range(0,1)]
    public float Friction;

    protected Rigidbody rigidbody;
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rigidbody.velocity = rigidbody.velocity * (1 - Friction);
        rigidbody.angularVelocity = rigidbody.angularVelocity * (1 - Friction);
    }



}