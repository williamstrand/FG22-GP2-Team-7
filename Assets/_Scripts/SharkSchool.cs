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
                Debug.Log("Player has entered the shark school");
                break;
            
            case "Coconut":
                StartCoroutine(DisableCollider());
                break;
        }
    }

    IEnumerator DisableCollider()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(_disableDuration);
        _collider.enabled = true;
    }
}
