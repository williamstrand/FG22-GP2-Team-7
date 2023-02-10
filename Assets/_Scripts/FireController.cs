using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] FirePropagation _firePropagation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IgnitionSource"))
        {
            _firePropagation.IgniteFire();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("IgnitionSource"))
        {
            _firePropagation.PutOutFire();
        }
    }
}