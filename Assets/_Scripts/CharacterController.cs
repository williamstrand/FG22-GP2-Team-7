using System;
using UnityEngine;

[RequireComponent(typeof(InputHandler), typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    [Header("Move")]
    [Tooltip("Speed if acceleration is on")]
    [Range(0, 100)][SerializeField] float _maxSpeed = 5;
    [Tooltip("Time in seconds until max speed from stationary position")]
    [Range(0.001f, 2)][SerializeField] float _accelerationTime = .15f;
    [Tooltip("Time in seconds until no speed from max speed")]
    [Range(0.001f, 2)][SerializeField] float _decelerationTime = .3f;
    [Tooltip("Should acceleration and deceleration be used")]
    [SerializeField] bool _useAcceleration = true;
    [Range(0, 10)][SerializeField] float _rotationSpeed = 2;

    [Header("Jump")]
    [Range(0, 50)][SerializeField] float _jumpForce = 5;
    [SerializeField] protected LayerMask _groundLayer;
    [Range(0, 100)][SerializeField] float _gravityScale = 1;
    [Tooltip("The distance from the middle of the character to check for ground.")]
    [Range(0, 2)][SerializeField] float _groundCheckDistance = .5f;
    [Tooltip("The radius of the ground check.")]
    [Range(0, 2)][SerializeField] float _groundCheckRadius = .45f;
    [Space(15)]
    [Tooltip("Should character be affected by gravity?")]
    [SerializeField] protected bool _applyGravity = true;
    float _currentSpeed;
    Vector2 _lastDirection = Vector2.zero;

    // References
    protected InputHandler _inputHandler;
    protected Rigidbody _rb;
    protected Collider _collider;

    protected virtual void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        FindCollider();
    }

    void FindCollider()
    {
        if (TryGetComponent(out _collider)) return;

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out _collider))
            {
                return;
            }
        }

        Debug.LogError($"No collider on {name} or it's children");
    }

    void OnEnable()
    {
        _inputHandler.OnJump += Jump;
        _inputHandler.OnInteract += Interact;
        _inputHandler.OnPlayerAction += PlayerAction;
    }

    void OnDisable()
    {
        _inputHandler.OnJump -= Jump;
        _inputHandler.OnInteract -= Interact;
        _inputHandler.OnPlayerAction -= PlayerAction;
    }

    protected virtual void Update()
    {
        Move(_inputHandler.MoveInput);
        Rotate();
    }

    protected virtual void FixedUpdate()
    {
        if (_applyGravity)
            ApplyGravityScale();
    }

    /// <summary>
    /// Applies gravity to the character.
    /// </summary>
    private void ApplyGravityScale()
    {
        _rb.AddForce(Vector3.down * _gravityScale, ForceMode.Acceleration);
    }

    /// <summary>
    /// Moves the character in the given direction.
    /// </summary>
    /// <param name="direction">the direction to move the character.</param>
    protected virtual void Move(Vector2 direction)
    {
        _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        if (!_useAcceleration)
        {
            var direction3D = new Vector3(direction.x, 0, direction.y);
            _rb.MovePosition(_rb.position + _maxSpeed * Time.deltaTime * direction3D);
        }
        else
        {
            if (direction == Vector2.zero)
            {
                _currentSpeed -= _maxSpeed / _decelerationTime * Time.deltaTime;
            }
            else
            {
                _lastDirection = direction;
                _currentSpeed += _maxSpeed / _accelerationTime * Time.deltaTime;
            }
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _maxSpeed);

            var direction3D = new Vector3(_lastDirection.x, 0, _lastDirection.y).normalized;
            _rb.MovePosition(_rb.position + _currentSpeed * Time.deltaTime * direction3D);
        }
    }

    protected virtual void Rotate()
    {
        var dir = _inputHandler.MoveInput;
        var targetDir = new Vector3(dir.x, 0, dir.y);

        if (targetDir == Vector3.zero) return;

        _rb.rotation = Quaternion.RotateTowards(_rb.rotation, Quaternion.LookRotation(targetDir, transform.up), _rotationSpeed * Time.deltaTime * 360);
    }
    
    /// <summary>
    /// Makes the character jump.
    /// </summary>
    protected virtual void Jump()
    {
        if (!IsGrounded()) return;

        if (_rb.velocity.y > 0) return;

        //_rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
    }

    /// <summary>
    /// Checks if character is on the ground.
    /// </summary>
    /// <returns>true if character is on the ground.</returns>
    protected bool IsGrounded() => Physics.CheckSphere(transform.position + Vector3.down * _groundCheckDistance, _groundCheckRadius, _groundLayer);

    /// <summary>
    /// Interact with an object.
    /// </summary>
    protected virtual void Interact()
    {

    }

    /// <summary>
    /// Performs the player specific action.
    /// </summary>
    protected virtual void PlayerAction()
    {
        Debug.Log("Player Action");
    }

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * _groundCheckDistance, _groundCheckRadius);
    }

#endif
}
