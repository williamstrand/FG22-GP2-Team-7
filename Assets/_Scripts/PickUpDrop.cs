using UnityEngine;

public class PickUpDrop : MonoBehaviour, IInteractable
{
    bool _itemIsPicked;

    Rigidbody _rigidbody;
    Collider _collider;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void Pickup(Transform player)
    {
        if (_itemIsPicked) return;

        _rigidbody.useGravity = false;
        _collider.enabled = false;
        _rigidbody.isKinematic = true;

        var pickupPoint = player.Find("PickupPoint");
        transform.position = pickupPoint.position;
        transform.parent = pickupPoint.transform;

        _itemIsPicked = true;
    }

    public void Drop()
    {
        transform.parent = null;
        _rigidbody.useGravity = true;
        _collider.enabled = true;
        _itemIsPicked = false;
        _rigidbody.isKinematic = false;
    }
}
