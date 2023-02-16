using UnityEngine;

public class FirstTargetHit : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coconut"))
        {
            _animator.SetBool("isHit", true);
        }
    }
}