using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    Animator _animator;
    [SerializeField] float _cooldown = 1;
    float _currentCooldown;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Hit()
    {
        if (_currentCooldown > 0) return;

        _currentCooldown = _cooldown;
        _animator.SetTrigger("isHit");

        // TODO: Add functionality
    }

    void Update()
    {
        if (_currentCooldown > 0)
            _currentCooldown -= Time.deltaTime;
    }
}
