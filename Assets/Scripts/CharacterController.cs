using System;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(InputHandler), typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    const float GroundCheckDistance = .5f;
    const float GroundCheckRadius = .45f;

    [Header("Move")]
    [Tooltip("Speed if acceleration is on")]
    [Range(0, 100)][SerializeField] float _maxSpeed = 5;
    [Tooltip("Time in seconds until max speed from stationary position")]
    [Range(0.001f, 2)][SerializeField] float _accelerationTime = .04f;
    [Tooltip("Time in seconds until no speed from max speed")]
    [Range(0.001f, 2)][SerializeField] float _decelerationTime = .05f;
    [Tooltip("Should acceleration and deceleration be used")]
    [SerializeField] bool _useAcceleration = true;
    
    [Header("Jump")]
    [SerializeField] float _jumpForce = 5;
    [SerializeField] LayerMask _groundLayer;

    float _currentSpeed;
    Vector2 _lastDirection = Vector2.zero;
    
    // References
    InputHandler _inputHandler;
    Rigidbody _rb;


    void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _rb = GetComponent<Rigidbody>();
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

    void Update()
    {
        Move(_inputHandler.MoveInput);
    }

    /// <summary>
    /// Moves the character in the given direction.
    /// </summary>
    /// <param name="direction">the direction to move the character.</param>
    void Move(Vector2 direction)
    {
        if (!_useAcceleration)
        {
            var direction3D = new Vector3(direction.x, 0, direction.y);
            _rb.MovePosition(_rb.position + _maxSpeed * Time.deltaTime * direction3D);
        }
        else
        {
            var direction3D = new Vector3(_lastDirection.x, 0, _lastDirection.y).normalized;
            if (direction == Vector2.zero)
            {
                _currentSpeed -= 1 / _decelerationTime * Time.deltaTime;
            }
            else
            {
                _lastDirection = direction;
                _currentSpeed += 1 / _accelerationTime * Time.deltaTime;
            }

            _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _maxSpeed);
            _rb.MovePosition(_rb.position + _currentSpeed * Time.deltaTime * direction3D);
        }
    }

    /// <summary>
    /// Makes the character jump.
    /// </summary>
    void Jump()
    {
        if (!IsGrounded()) return;

        if (_rb.velocity.y > 0) return;
        
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Checks if character is on the ground.
    /// </summary>
    /// <returns>true if character is on the ground.</returns>
    bool IsGrounded() => Physics.CheckSphere(transform.position + Vector3.down * GroundCheckDistance, GroundCheckRadius, _groundLayer);

    void Interact()
    {
        Debug.Log("Interact");
    }
    
    protected virtual void PlayerAction()
    {
        Debug.Log("Player Action");
    }

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * GroundCheckDistance, GroundCheckRadius);
    }

#endif
}
