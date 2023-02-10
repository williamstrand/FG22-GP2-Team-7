using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(ParticleSystem))]
public class WaterPower : MonoBehaviour
{
    LineRenderer _aimLineRenderer;
    ParticleSystem _waterPowerParticles;

    void Awake()
    {
        _aimLineRenderer = GetComponent<LineRenderer>();
        _waterPowerParticles = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// Toggles water shooting.
    /// </summary>
    public void ToggleShooting()
    {
        if (_waterPowerParticles.isEmitting)
        {
            _waterPowerParticles.Stop();
        }
        else
        {
            _waterPowerParticles.Play();
        }
    }

    /// <summary>
    /// Turns the player.
    /// </summary>
    /// <param name="direction">the direction to turn in. less than 0 is left, greater than 0 is right.</param>
    /// <param name="speed">the speed at which the player will rotate.</param>
    public void Turn(float direction, float speed)
    {
        transform.rotation *= Quaternion.Euler(0, direction * speed, 0);
        UpdateAim();
    }

    /// <summary>
    /// Activates the water power.
    /// </summary>
    public void ActivateWaterPower()
    {
        AimEnabled(true);
    }

    /// <summary>
    /// Deactivates the water power.
    /// </summary>
    public void DeactivateWaterPower()
    {
        AimEnabled(false);
        _waterPowerParticles.Stop();
    }

    /// <summary>
    /// Updates the aim line renderer.
    /// </summary>
    void UpdateAim()
    {
        _aimLineRenderer.SetPosition(0, transform.position);
        _aimLineRenderer.SetPosition(1, transform.position + transform.forward * 10 + transform.up * 2);
    }

    /// <summary>
    /// Enables or disables the aim line renderer.
    /// </summary>
    /// <param name="isEnabled"></param>
    void AimEnabled(bool isEnabled)
    {
        _aimLineRenderer.enabled = isEnabled;
        UpdateAim();
    }

    void OnParticleCollision(GameObject other)
    {
        // TODO: Add particle collision detection
    }
}
