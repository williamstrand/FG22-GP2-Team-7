using System;
using UnityEngine;

public class FireController : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IgnitableObject"))
        {
            IgnitedObject ignitedObject = other.GetComponent<IgnitedObject>();
            ignitedObject.Ignite();
            

            FirePropagation firePropagation = other.GetComponent<FirePropagation>();
            if (firePropagation != null)
            {
                firePropagation.IgniteFire();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("IgnitableObject"))
        {
            IgnitedObject ignitedObject = other.GetComponent<IgnitedObject>();
            ignitedObject.Extinguish();

            FirePropagation firePropagation = other.GetComponent<FirePropagation>();
            if (firePropagation != null)
            {
                firePropagation.PutOutFire();
            }
        }
    }

    
}