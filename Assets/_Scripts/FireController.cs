using System;
using UnityEngine;

public class FireController : MonoBehaviour
{
    private bool _isInsideIgnitableObjectTrigger = false;
    private TorchIgnition _torchIgnition;

    private void Start()
    {
        _torchIgnition = GetComponent<TorchIgnition>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IgnitableObject") && _torchIgnition._isTorchLit)
        {
            _isInsideIgnitableObjectTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("IgnitableObject"))
        {
            _isInsideIgnitableObjectTrigger = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isInsideIgnitableObjectTrigger && Input.GetKeyDown(KeyCode.Keypad2))
        {
            IgnitedObject ignitedObject = other.GetComponentInParent<IgnitedObject>();
            ignitedObject.Ignite();

            FirePropagation firePropagation = other.GetComponentInParent<FirePropagation>();
            if (firePropagation != null)
            {
                firePropagation.IgniteFire();
            }

            _torchIgnition.PutOutTorchFire();
        }
    }
}