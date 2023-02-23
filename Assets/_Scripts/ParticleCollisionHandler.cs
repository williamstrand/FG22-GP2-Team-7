using System;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    public event Action<GameObject> ParticleCollisionEvent;

    void OnParticleCollision(GameObject other)
    {
        ParticleCollisionEvent?.Invoke(other);
    }
}
