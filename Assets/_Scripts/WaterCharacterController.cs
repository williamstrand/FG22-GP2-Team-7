using System.Collections;
using UnityEngine;

public class WaterCharacterController : CharacterController
{

    WaterPlayerState _playerState = WaterPlayerState.Default;
    bool _canDive = true;

    [Header("Diving")]
    [Range(0, 5)][SerializeField] float _diveSpeed = 1;
    [Range(0, 5)][SerializeField] float _diveDepth = 2f;
    

    public enum WaterPlayerState
    {
        Default,
        Diving
    }

    protected override void PlayerAction()
    {
        if (!CanDive()) return;

        switch (_playerState)
        {
            case WaterPlayerState.Default:
                if (!IsGrounded()) return;
                Dive();
                break;

            case WaterPlayerState.Diving:
                Resurface();
                break;
        }
    }

    protected override void Jump()
    {
        switch (_playerState)
        {
            case WaterPlayerState.Default:
                base.Jump();
                break;
            case WaterPlayerState.Diving:
                break;
        }
    }

    /// <summary>
    /// Dive down into the water.
    /// </summary>
    void Dive()
    {
        if (_rb.SweepTest(Vector3.down, out _, _diveDepth)) return;

        StartCoroutine(DiveRoutine(true));
    }

    /// <summary>
    /// Return to the surface.
    /// </summary>
    void Resurface()
    {
        if (_rb.SweepTest(Vector3.up, out _, _diveDepth)) return;

        StartCoroutine(DiveRoutine(false));
    }

    IEnumerator DiveRoutine(bool down)
    {
        _applyGravity = false;
        _canDive = false;
        var waterCollider = Physics.OverlapSphere(transform.position, _diveDepth, _groundLayer)[0];
        Physics.IgnoreCollision(_collider, waterCollider, down);
        var targetY = down ? transform.position.y - _diveDepth : transform.position.y + _diveDepth;
        var startY = transform.position.y;

        var lerp = 0f;
        while (lerp < 1)
        {
            _rb.velocity = Vector3.zero;
            while (_rb.SweepTest((down) ? Vector3.down : Vector3.up, out _, _diveDepth)) yield return null;

            lerp += Time.deltaTime * _diveSpeed;

            var moveSpeed = Time.deltaTime * 5 * _inputHandler.MoveInput;
            var targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            var startPosition = new Vector3(transform.position.x, startY, transform.position.z);

            transform.position = Vector3.Lerp(startPosition, targetPosition, lerp) + new Vector3(moveSpeed.x, 0, moveSpeed.y);
            yield return null;
        }

        _canDive = true;
        _playerState = down ? WaterPlayerState.Diving : WaterPlayerState.Default;
        _applyGravity = !down;
    }

    bool CanDive() => _canDive;
}
