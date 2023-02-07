using UnityEngine;

public class Catapult : MonoBehaviour, IInteractable
{
    private const float GravityScale = 9.81f;
    private const int MaxAimDrawAmount = 1000;
    
    float _cooldownTimer;
    [SerializeField] float _cooldown;
    [SerializeField] float _minForce;
    [SerializeField] float _maxForce;
    [SerializeField] Rigidbody _coconut;
    [SerializeField] Transform _shootPoint;
    [SerializeField] Transform _cockpit;
    public float AimAngle { get; private set; } = 45f;

    LineRenderer _lineRenderer;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void Fire()
    {
        if(_cooldownTimer > 0) return;
        _cooldownTimer = _cooldown;

        SetAim(AimAngle);
        LoadCoconut(_coconut);
        
        _coconut.transform.SetParent(null);
        _coconut.AddForce(_shootPoint.forward * _maxForce, ForceMode.VelocityChange);
        
        ClearAim();
    }

    public void LoadCoconut(Rigidbody coconut)
    {
        coconut.velocity = Vector3.zero;
        coconut.rotation = Quaternion.identity;
        coconut.transform.position = _shootPoint.position;
        coconut.transform.SetParent(_shootPoint);
        _coconut = coconut;
    }

    public void EnterCatapult(LandCharacterController player)
    {
        player.transform.position = _cockpit.position;
        player.transform.rotation = _cockpit.rotation;
        player.transform.SetParent(_cockpit);
        DrawAim();
    }

    public void ExitCatapult(LandCharacterController player)
    {
        player.transform.SetParent(null);
        ClearAim();
    }

    public void SetAim(float angle)
    {
        var newAngle = Mathf.Clamp(angle, 25, 80);
        _shootPoint.localEulerAngles = new Vector3(-newAngle, 0, 0);
        AimAngle = newAngle;
        DrawAim();
    }

    public void SetRotation(float rotation)
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + rotation, 0);
        DrawAim();
    }

    void DrawAim()
    {
        ClearAim();
        for (int i = 0; i < MaxAimDrawAmount; i++)
        {
            var pos = GetPosition(i, _maxForce);
            if (pos.y < transform.position.y) break;
            _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(i, pos);
        }
    }

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
