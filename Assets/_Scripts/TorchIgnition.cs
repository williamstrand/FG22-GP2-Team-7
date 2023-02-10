using System;
using UnityEngine;

public class TorchIgnition : MonoBehaviour
{
    [SerializeField] private ParticleSystem _fireParticles;

    private void Start()
    {
        _fireParticles.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IgnitionSource"))
        {
            _fireParticles.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("IgnitableObject"))
        {
            _fireParticles.gameObject.SetActive(false);
        }
    }
}