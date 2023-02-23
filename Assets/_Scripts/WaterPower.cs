using System.Collections;
using UnityEngine;

public class WaterPower : MonoBehaviour
{
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Vector2 _minMaxShootAngle;

    LineRenderer _lineRenderer;
    [SerializeField] ParticleSystem _waterPowerParticles;
    [SerializeField] GameObject _particleEmitter;
    public bool IsShooting { get; private set; }

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _particleEmitter.GetComponent<ParticleCollisionHandler>().ParticleCollisionEvent += OnParticleCollision;
    }

    /// <summary>
    /// Toggles water shooting.
    /// </summary>
    public void ToggleShooting()
    {
        if (_waterPowerParticles.isEmitting)
        {
            IsShooting = false;
            _waterPowerParticles.Stop();
        }
        else
        {
            IsShooting = true;
            StartCoroutine(ShootDelay(.25f));
        }
    }

    IEnumerator ShootDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _waterPowerParticles.Play();
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

    public void Aim(float direction, float speed)
    {
        var shape = _waterPowerParticles.shape;
        shape.rotation = new Vector3(Mathf.Clamp(shape.rotation.x + -direction * speed, _minMaxShootAngle.x, _minMaxShootAngle.y), 0, 0);
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
        IsShooting = false;
        AimEnabled(false);
        _waterPowerParticles.Stop();
    }

    /// <summary>
    /// Updates the aim line renderer.
    /// </summary>
    void UpdateAim()
    {
        ClearAim();
        var angle = -_waterPowerParticles.shape.rotation.x;
        var main = _waterPowerParticles.main;
        var force = main.startSpeed.constant;
        for (int i = 0; i < 100; i++)
        {
            var pos = GetPosition(i / 10f, force, angle);
            var hit = Physics.OverlapSphere(pos, .1f, _groundLayer);
            _lineRenderer.positionCount++;
            if (hit.Length > 0)
            {
                _lineRenderer.SetPosition(i, hit[0].ClosestPointOnBounds(pos));
                break;
            }
            _lineRenderer.SetPosition(i, pos);
        }
    }

    /// <summary>
    /// Clears the aim.
    /// </summary>
    void ClearAim()
    {
        _lineRenderer.positionCount = 0;
    }

    /// <summary>
    /// Gets the position of the shot coconut at a specific x on the estimated trajectory.
    /// </summary>
    /// <param name="x">the x value.</param>
    /// <param name="force">the force the coconut is shot at.</param>
    /// <param name="angle">TODO:</param>
    /// <returns>a Vector3 representing a position in world space.</returns>
    Vector3 GetPosition(float x, float force, float angle)
    {
        var angleRad = angle * Mathf.Deg2Rad;
        var y = x * Mathf.Tan(angleRad) - 9.81f * x * x / (2 * force * force * Mathf.Cos(angleRad) * Mathf.Cos(angleRad));
        var vector = new Vector3(0, y, x);
        var shape = _waterPowerParticles.shape;
        var pos = transform.position + (shape.position.z * transform.forward + shape.position.y * transform.up);
        return pos + vector.x * transform.right + vector.y * transform.up + vector.z * transform.forward;
    }

    /// <summary>
    /// Enables or disables the aim line renderer.
    /// </summary>
    /// <param name="isEnabled"></param>
    void AimEnabled(bool isEnabled)
    {
        _lineRenderer.enabled = isEnabled;
        UpdateAim();
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<Target>(out var hit))
        {
            hit.Hit();
        }
    }
}
