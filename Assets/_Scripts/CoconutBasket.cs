using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CoconutBasket : MonoBehaviour
{
    [SerializeField] UnityEvent _puzzleComplete;
    Collider _coconutTrigger;
    [SerializeField] Transform _coconut1Position;
    [SerializeField] Transform _coconut2Position;
    AudioSource _audioSource;

    bool _coconut1;
    bool _coconut2;

    void Awake()
    {
        _coconutTrigger = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coconut")) return;
        
        if (!_coconut1)
        {
            _coconut1 = true;
            other.transform.position = _coconut1Position.position;
            other.transform.parent = transform.parent;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<Collider>().enabled = false;
            _audioSource.Play();
        }
        else if (!_coconut2)
        {
            _coconut2 = true;
            other.transform.position = _coconut2Position.position;
            other.transform.parent = transform.parent;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<Collider>().enabled = false;
            _audioSource.Play();
        }
        else
        {
            _coconutTrigger.enabled = false;
        }

        if (_coconut1 && _coconut2)
        {
            _puzzleComplete?.Invoke();
        }
    }
}
