using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Catapult : MonoBehaviour, IInteractable
{
    private const float GravityScale = 9.81f;
    private const int MaxAimDrawAmount = 1000;

    [Header("Catapult Stats")]
    [SerializeField] float _cooldown;
    float _cooldownTimer;
    [SerializeField] float _minForce;
    [SerializeField] float _maxForce;

    [Header("References")]
    [SerializeField] Rigidbody _coconut;
    [SerializeField] Transform _shootPoint;
    [SerializeField] Transform _cockpit;
    LineRenderer _lineRenderer;

    public float Force { get; private set; }
    public float AimAngle { get; private set; } = 45f;

    [Space(15)]
    [SerializeField] LayerMask _groundLayer;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    /// <summary>
    /// Fires the catapult.
    /// </summary>
    public void Fire()
    {
        if (_cooldownTimer > 0) return;
        _cooldownTimer = _cooldown;

        SetAim(AimAngle);
        // TODO: Add manual loading of coconut.
        LoadCoconut(_coconut);

        _coconut.transform.SetParent(null);
        _coconut.AddForce(_shootPoint.forward * Force, ForceMode.VelocityChange);

        ClearAim();
    }

    /// <summary>
    /// Loads a coconut in to the catapult.
    /// </summary>
    /// <param name="coconut">the coconut.</param>
    public void LoadCoconut(Rigidbody coconut)
    {
        coconut.velocity = Vector3.zero;
        coconut.rotation = Quaternion.identity;
        coconut.transform.position = _shootPoint.position;
        coconut.transform.SetParent(_shootPoint);
        _coconut = coconut;
    }

    /// <summary>
    /// Makes player enter the catapult.
    /// </summary>
    /// <param name="player">the player.</param>
    public void EnterCatapult(LandCharacterController player)
    {
        player.transform.position = _cockpit.position;
        player.transform.rotation = _cockpit.rotation;
        player.transform.SetParent(_cockpit);
        DrawAim();
    }

    /// <summary>
    /// Makes player exit the catapult.
    /// </summary>
    /// <param name="player">the player.</param>
    public void ExitCatapult(LandCharacterController player)
    {
        player.transform.SetParent(null);
        ClearAim();
    }

    /// <summary>
    /// Set the shoot angle of the catapult.
    /// </summary>
    /// <param name="angle">the angle in degrees.</param>
    public void SetAim(float angle)
    {
        var newAngle = Mathf.Clamp(angle, 25, 80);
        _shootPoint.localEulerAngles = new Vector3(-newAngle, 0, 0);
        AimAngle = newAngle;
        DrawAim();
    }

    /// <summary>
    /// Set the rotation of the catapult.
    /// </summary>
    /// <param name="rotation">the rotation in degrees.</param>
    public void SetRotation(float rotation)
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + rotation, 0);
        DrawAim();
    }

    /// <summary>
    /// Set the force of the catapult.
    /// </summary>
    /// <param name="force">the force.</param>
    public void SetForce(float force)
    {
        Force = Mathf.Clamp(force, _minForce, _maxForce);
        DrawAim();
    }

    /// <summary>
    /// Draws the aim of the catapult.
    /// </summary>
    void DrawAim()
    {
        ClearAim();
        for (int i = 0; i < MaxAimDrawAmount; i++)
        {
            var pos = GetPosition(i / 10f, Force);
            var hit = Physics.OverlapSphere(pos, .1f, _groundLayer);
            _lineRenderer.positionCount++;
            if (hit.Length > 0)
            {
                _lineRenderer.SetPosition(i, hit[0].ClosestPoint(pos));
                break;
            }
            _lineRenderer.SetPosition(i, pos);
        }
    }

    /// <summary>
    /// Clears the aim of the catapult.
    /// </summary>
    void ClearAim()
    {
        _lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Gets the position of the shot coconut at a specific x on the estimated trajectory.
    /// </summary>
    /// <param name="x">the x value.</param>
    /// <param name="force">the force the coconut is shot at.</param>
    /// <returns>a Vector3 representing a position in world space.</returns>
    Vector3 GetPosition(float x, float force)
    {
        var angleRad = AimAngle * Mathf.Deg2Rad;
        var y = x * Mathf.Tan(angleRad) - GravityScale * x * x / (2 * force * force * Mathf.Cos(angleRad) * Mathf.Cos(angleRad));
        var vector = new Vector3(0, y, x);
        return _shootPoint.position + vector.x * transform.right + vector.y * transform.up + vector.z * transform.forward;
    }

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        var radius = 0.1f;
        for (int i = 0; i < 100; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(GetPosition(i, _maxForce), radius);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(GetPosition(i, _minForce), radius);
        }
    }

#endif
}
