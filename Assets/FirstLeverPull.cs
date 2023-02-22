using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLeverPull : MonoBehaviour
{
    public Animator _animator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.Play("1stLeverDown");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.Play("1stLeverUp");
        }
    }
}