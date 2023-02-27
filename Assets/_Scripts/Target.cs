using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    Animator _animator;
    [SerializeField] float _cooldown = 1;
    float _currentCooldown;
    [SerializeField] UnityEvent _onHit;
    AudioSource _audioSource;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Hit()
    {
        if (_currentCooldown > 0) return;

        _currentCooldown = _cooldown;
        _animator.SetTrigger("isHit");
        _audioSource.Play();

        _onHit?.Invoke();
    }

    void Update()
    {
        if (_currentCooldown > 0)
            _currentCooldown -= Time.deltaTime;
    }
}
