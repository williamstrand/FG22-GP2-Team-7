using UnityEngine;

public class LandCharacterController : CharacterController
{
    [Header("Climbing")]
    [Range(0, 2)][SerializeField] float _climbSpeed = 1f;
    Climbable _currentClimbable;

    [Header("Catapult")]
    [SerializeField] float _catapultAimSpeed = 1f;
    Catapult _catapult;

    [Space(15)]
    [SerializeField] float _interactRange = 1f;

    LandPlayerState _playerState = LandPlayerState.Default;

    public enum LandPlayerState
    {
        Default,
        Climbing,
        Catapult
    }

    protected override void FixedUpdate()
    {
        switch (_playerState)
        {
            case LandPlayerState.Default:
                base.FixedUpdate();
                break;

            default:
                break;
        }
    }

    protected override void Rotate()
    {
        switch (_playerState)
        {
            case LandPlayerState.Default:
                base.Rotate();
                break;

        }
    }

    protected override void Move(Vector2 direction)
    {
        switch (_playerState)
        {
            case LandPlayerState.Default:
                base.Move(direction);
                break;

            case LandPlayerState.Climbing:
                switch (direction.y)
                {
                    case 0:
                        _currentClimbable.Climb(0, _climbSpeed);
                        break;
                    case > 0:
                        _currentClimbable.Climb(Mathf.CeilToInt(direction.y), _climbSpeed);
                        break;
                    case < 0:
                        _currentClimbable.Climb(Mathf.FloorToInt(direction.y), _climbSpeed);
                        break;
                }
                break;

            case LandPlayerState.Catapult:
                _catapult.SetAim(_catapult.AimAngle + direction.y * _catapultAimSpeed);
                _catapult.SetForce(_catapult.Force + direction.x * _catapultAimSpeed * .1f);
                //_catapult.SetRotation(direction.x * _catapultAimSpeed * .3f);
                break;
        }
    }

    /// <summary>
    /// Interact with the closest interactable object.
    /// </summary>
    protected override void Interact()
    {
        var closeObjects = Physics.OverlapSphere(transform.position, _interactRange);
        foreach (var closeObject in closeObjects)
        {
            if (!closeObject.TryGetComponent(out IInteractable interactable)) continue;
            switch (interactable)
            {
                case Climbable when _playerState == LandPlayerState.Climbing:
                    _currentClimbable.StopClimb();
                    _currentClimbable = null;
                    StopClimb();
                    break;

                case Climbable climbable:
                    climbable.StartClimb(this);
                    _currentClimbable = climbable;
                    StartClimb();
                    break;

                case Catapult when _playerState == LandPlayerState.Catapult:
                    _catapult.ExitCatapult(this);
                    _catapult = null;
                    _playerState = LandPlayerState.Default;
                    break;

                case Catapult catapult:
                    catapult.EnterCatapult(this);
                    _playerState = LandPlayerState.Catapult;
                    _catapult = catapult;
                    break;
            }
        }
    }

    protected override void PlayerAction()
    {
        switch (_playerState)
        {
            case LandPlayerState.Default:
                base.PlayerAction();
                break;

            case LandPlayerState.Catapult:
                _catapult.Fire();
                break;
        }
    }

    /// <summary>
    /// Start climbing.
    /// </summary>
    void StartClimb()
    {
        _playerState = LandPlayerState.Climbing;
        _collider.enabled = false;
        _rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// Stop climbing.
    /// </summary>
    void StopClimb()
    {
        _playerState = LandPlayerState.Default;
        _collider.enabled = true;
        _rb.velocity = Vector3.zero;
    }
}
