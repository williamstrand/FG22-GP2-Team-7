using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SharkSchool : MonoBehaviour
{
    Collider _collider;
    [SerializeField] float _disableDuration = 1;

    void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                other.GetComponent<CharacterController>().Respawn();
                break;

            case "Coconut":
                StartCoroutine(DisableCollider());
                break;
        }
    }

    /// <summary>
    /// Disables collider for a set duration.
    /// </summary>
    IEnumerator DisableCollider()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(_disableDuration);
        _collider.enabled = true;
    }
}
