using UnityEngine;

public class PickUpDrop : MonoBehaviour, IInteractable
{
    Rigidbody _rigidbody;
    Collider _collider;
    [SerializeField] bool _respawnInWater = true;
    Vector3 _respawnPoint;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _respawnPoint = transform.position;
    }

    /// <summary>
    /// Pick up object and set it's parent to pickupPoint.
    /// </summary>
    /// <param name="pickupPoint"></param>
    public void Pickup(Transform pickupPoint)
    {
        _rigidbody.useGravity = false;
        //_collider.enabled = false;
        _rigidbody.isKinematic = true;
        
        transform.position = pickupPoint.position;
        transform.parent = pickupPoint.transform;
    }

    /// <summary>
    /// Drop object.
    /// </summary>
    public void Drop()
    {
        transform.parent = null;
        _rigidbody.useGravity = true;
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Water") || other.gameObject.CompareTag("Rocks") && _respawnInWater )
        {
            transform.position = _respawnPoint;
        }
    }
}
