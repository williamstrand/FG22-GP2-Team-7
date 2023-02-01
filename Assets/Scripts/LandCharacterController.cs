using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandCharacterController : CharacterController
{
    LandPlayerState _playerState = LandPlayerState.Default;
    [Header("Climbing")]
    [SerializeField] float _climbSpeed = 1f;

    [Space(15)]
    [SerializeField] Collider _collider;

    Climbable _currentClimbable;

    public enum LandPlayerState
    {
        Default,
        Climbing
    }

    protected override void FixedUpdate()
    {
        switch (_playerState)
        {
            case LandPlayerState.Default:
                base.FixedUpdate();
                break;

            case LandPlayerState.Climbing:
                break;

            default:
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
                _currentClimbable.Climb((int)direction.y, _climbSpeed);
                break;

            default:
                break;
        }
    }

    protected override void Interact()
    {
        var closeObjects = Physics.OverlapSphere(transform.position, 1);
        foreach (var closeObject in closeObjects)
        {
            if (closeObject.TryGetComponent(out Climbable climbable))
            {
                if (_playerState == LandPlayerState.Climbing)
                {
                    _currentClimbable.StopClimb();
                    _currentClimbable = null;
                    StopClimb();
                }
                else
                {
                    climbable.StartClimb(this);
                    _currentClimbable = climbable;
                    StartClimb();
                }
            }
        }
    }

    void StartClimb()
    {
        _playerState = LandPlayerState.Climbing;
        _collider.enabled = false;
        _rb.velocity = Vector3.zero;
    }

    void StopClimb()
    {
        _playerState = LandPlayerState.Default;
        _collider.enabled = true;
    }
}
