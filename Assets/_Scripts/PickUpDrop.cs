using UnityEngine;

public class PickUpDrop : MonoBehaviour, IInteractable
{
    Rigidbody _rigidbody;
    Collider _collider;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void Pickup(Transform pickupPoint)
    {
        _rigidbody.useGravity = false;
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
        
        transform.position = pickupPoint.position;
        transform.parent = pickupPoint.transform;
    }

    public void Drop()
    {
        transform.parent = null;
        _rigidbody.useGravity = true;
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
    }
}
