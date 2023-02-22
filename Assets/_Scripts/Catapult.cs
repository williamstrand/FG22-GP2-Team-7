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
    [SerializeField] float _fireSoundVolume;
    [SerializeField] float _loadSoundVolume;

    [Header("References")]
    PickUpDrop _coconut;
    [SerializeField] Transform _shootPoint;
    [SerializeField] Transform _cockpit;
    [SerializeField] Transform _target;
    [SerializeField] Transform _coconutPosition;
    [SerializeField] SoundHolder _soundHolder;
    LineRenderer _lineRenderer;
    Animator _animator;
    [SerializeField] AudioSource _loadAudioSource;
    [SerializeField] AudioSource _fireAudioSource;

    public float Force { get; private set; }
    public float AimAngle { get; private set; } = 45f;
    bool _canFire;

    [Space(15)]
    [SerializeField] LayerMask _groundLayer;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Fires the catapult.
    /// </summary>
    public void Fire()
    {
        if (_coconut == null) return;
        if (_cooldownTimer > 0) return;
        if (!_canFire) return;
        _cooldownTimer = _cooldown;

        SetAim(AimAngle);

        _coconut.transform.SetParent(null);
        var rb = _coconut.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(_shootPoint.forward * Force, ForceMode.VelocityChange);
        Physics.IgnoreCollision(_coconut.GetComponent<Collider>(), GetComponent<Collider>(), false);

        _coconut.GetComponentInChildren<UIInteraction>().gameObject.SetActive(true);

        _animator.SetTrigger("Fire");
        _coconut = null;
        _fireAudioSource.Play();

        ClearAim();
    }

    /// <summary>
    /// Loads a coconut in to the catapult.
    /// </summary>
    /// <param name="coconut">the coconut.</param>
    public void LoadCoconut(PickUpDrop coconut)
    {
        _animator.SetTrigger("Load");
        _loadAudioSource.Play();
        var rb = coconut.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
        coconut.transform.position = _coconutPosition.position;
        coconut.transform.SetParent(_coconutPosition);
        coconut.transform.localPosition = new Vector3(0, .8f / 100f, 0);
        _coconut = coconut;
        Physics.IgnoreCollision(coconut.GetComponent<Collider>(), GetComponent<Collider>(), true);
        coconut.GetComponent<Rigidbody>().isKinematic = true;
        _canFire = false;
    }

    public void LoadComplete()
    {
        _coconut.transform.position = _shootPoint.position;
        _canFire = true;
    }

    /// <summary>
    /// Makes player enter the catapult.
    /// </summary>
    /// <param name="player">the player.</param>
    public void EnterCatapult(LandCharacterController player)
    {
        player.transform.SetPositionAndRotation(_cockpit.position, _cockpit.rotation);
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
                _target.gameObject.SetActive(true);
                var prevPos = GetPosition((i - 1) / 10f, Force);
                _target.position = hit[0].ClosestPoint(prevPos) + Vector3.up * .05f;
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
        _target.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
        }

        if (_coconut)
        {
            _coconut.transform.localScale = _coconutPosition.localScale / 100f;
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
