using System.Collections;
using UnityEngine;

[RequireComponent(typeof(WaterPower))]
public class WaterCharacterController : CharacterController
{

    WaterPlayerState _playerState = WaterPlayerState.Default;
    bool _canDive = true;
    bool _canJump = true;

    [Header("Diving")]
    [Range(0, 5)][SerializeField] float _diveSpeed = 1;
    [Range(0, 5)][SerializeField] float _diveDepth = 2f;

    [Header("Water Power")]
    [Range(1, 5)][SerializeField] float _waterPowerTurnSpeed = 1;
    [Range(1, 5)][SerializeField] float _waterPowerAimSpeed = 1;
    [SerializeField] AudioSource _waterSquirtSource;
    [SerializeField] float _waterSquirtVolume = 1;

    [Header("Push Pull")]
    Pushable _pushable;
    WaterPower _waterPower;


    public enum WaterPlayerState
    {
        Default,
        Diving,
        WaterPower,
        Pushing
    }

    protected override void Awake()
    {
        base.Awake();
        _waterPower = GetComponent<WaterPower>();
    }

    protected override void Update()
    {
        base.Update();

        if (_playerState != WaterPlayerState.WaterPower) return;

        PlaySound(_waterSquirtSource, _waterSquirtVolume, _waterPower.IsShooting);
    }

    protected override void PlayerAction()
    {

        switch (_playerState)
        {
            case WaterPlayerState.Default:
                if (!CanDive()) return;
                if (!IsGrounded()) return;
                Dive();
                PlaySound(_footstepAudioSource, _footStepVolume, false);
                break;

            case WaterPlayerState.Diving:
                if (!CanDive()) return;
                Resurface();
                break;

            case WaterPlayerState.WaterPower:
                _waterPower.ToggleShooting();
                _animator.SetBool("Spitting", _waterPower.IsShooting);
                break;
        }
    }



    protected override void Interact()
    {
        switch (_playerState)
        {
            case WaterPlayerState.Default:
                DefaultInteract();
                break;

            case WaterPlayerState.WaterPower:
                _playerState = WaterPlayerState.Default;
                _waterPower.DeactivateWaterPower();
                break;

            case WaterPlayerState.Pushing:
                _pushable.StopPush();
                _playerState = WaterPlayerState.Default;
                _animator.SetBool("Pushing", false);
                break;
        }
    }

    void DefaultInteract()
    {
        var closeObjects = Physics.OverlapSphere(transform.position, _interactRange);
        foreach (var closeObject in closeObjects)
        {
            if (!closeObject.TryGetComponent(out IInteractable interactable)) continue;
            switch (interactable)
            {
                case Pushable pushable:
                    _pushable = pushable;
                    pushable.StartPush(transform);
                    _playerState = WaterPlayerState.Pushing;
                    var rot = (new Vector3(pushable.transform.position.x, transform.position.y, pushable.transform.position.z) - transform.position).normalized;
                    transform.rotation = Quaternion.LookRotation(rot);
                    _animator.SetBool("Pushing", true);
                    return;
            }
        }

        _playerState = WaterPlayerState.WaterPower;
        _waterPower.ActivateWaterPower();
        _rb.velocity = Vector3.zero;
        _currentSpeed = 0;
        PlaySound(_footstepAudioSource, 0, false);
    }

    protected override void Jump()
    {
        switch (_playerState)
        {
            case WaterPlayerState.Default:
                if (!_canJump) break;
                base.Jump();
                _canJump = false;
                break;
        }
    }

    public void JumpFinished()
    {
        _canJump = true;
    }

    protected override void Move(Vector2 direction)
    {
        switch (_playerState)
        {
            case WaterPlayerState.Default:
                base.Move(direction);
                break;

            case WaterPlayerState.Diving:
                base.Move(direction);
                break;

            case WaterPlayerState.WaterPower:
                _waterPower.Turn(direction.x, _waterPowerTurnSpeed);
                _waterPower.Aim(direction.y, _waterPowerAimSpeed);
                break;

            case WaterPlayerState.Pushing:
                if (_pushable == null) return;
                if (direction.y > 0)
                {
                    _pushable.Push(5);
                }
                if (direction.x != 0)
                {
                    _pushable.Turn(direction.x);
                }
                break;
        }
    }

    protected override void Rotate()
    {
        switch (_playerState)
        {
            case WaterPlayerState.Default:
                base.Rotate();
                break;

            case WaterPlayerState.Diving:
                base.Rotate();
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
        _playerState = down ? WaterPlayerState.Diving : WaterPlayerState.Default;
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

            var targetPosition = new Vector3(_rb.position.x, targetY, _rb.position.z);
            var startPosition = new Vector3(_rb.position.x, startY, _rb.position.z);

            _rb.position = Vector3.Lerp(startPosition, targetPosition, lerp);
            yield return null;
        }

        _canDive = true;
        _applyGravity = !down;
    }

    bool CanDive() => _canDive;
}
