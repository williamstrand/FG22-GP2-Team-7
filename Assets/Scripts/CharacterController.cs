using UnityEngine;

[RequireComponent(typeof(InputHandler), typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    const float GroundCheckDistance = .5f;
    const float GroundCheckRadius = .45f;

    [Header("Stats")]
    [SerializeField] float _speed = 5;
    [SerializeField] float _jumpForce = 5;

    [Space(20)]
    [SerializeField] LayerMask _groundLayer;

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
        _inputHandler.OnMove += Move;
        _inputHandler.OnJump += Jump;
    }

    void OnDisable()
    {
        _inputHandler.OnMove -= Move;
        _inputHandler.OnJump -= Jump;
    }

    /// <summary>
    /// Moves the character in the given direction.
    /// </summary>
    /// <param name="direction">the direction to move the character.</param>
    void Move(Vector2 direction)
    {
        var direction3D = new Vector3(direction.x, 0, direction.y);
        _rb.MovePosition(_rb.position + _speed * Time.deltaTime * direction3D);
    }

    /// <summary>
    /// 
    /// </summary>
    void Jump()
    {
        if (!IsGrounded()) return;

        _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
        //_rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Checks if character is on the ground.
    /// </summary>
    /// <returns>true if character is on the ground.</returns>
    bool IsGrounded() => Physics.CheckSphere(transform.position + Vector3.down * GroundCheckDistance, GroundCheckRadius, _groundLayer);

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * GroundCheckDistance, GroundCheckRadius);
    }

#endif
}
