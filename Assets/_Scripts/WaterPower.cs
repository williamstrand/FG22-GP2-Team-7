using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(ParticleSystem))]
public class WaterPower : MonoBehaviour
{
    LineRenderer _aimLineRenderer;
    [SerializeField] ParticleSystem _waterPowerParticles;

    void Awake()
    {
        _aimLineRenderer = GetComponent<LineRenderer>();
    }

    public void Shoot()
    {
        if (_waterPowerParticles.isPlaying)
        {
            _waterPowerParticles.Stop();
        }
        else
        {
            _waterPowerParticles.Play();
        }

    }

    public void Turn(float direction, float speed)
    {
        transform.rotation *= Quaternion.Euler(0, direction * speed, 0);
        UpdateAim();
    }

    public void ActivateWaterPower()
    {
        AimEnabled(true);
    }

    public void DeactivateWaterPower()
    {
        AimEnabled(false);
        _waterPowerParticles.Stop();
    }

    void UpdateAim()
    {
        _aimLineRenderer.SetPosition(0, transform.position);
        _aimLineRenderer.SetPosition(1, transform.position + transform.forward * 10);
    }

    void AimEnabled(bool isEnabled)
    {
        _aimLineRenderer.enabled = isEnabled;
        UpdateAim();
    }

    void OnParticleCollision(GameObject other)
    {
        // TODO: Add particle collision detection
        Debug.Log("Hit " + other.name);
    }
}
