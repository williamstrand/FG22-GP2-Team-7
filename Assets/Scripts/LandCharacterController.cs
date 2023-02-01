using UnityEngine;

public class LandCharacterController : CharacterController
{
    [Header("Climbing")]
    [Range(0, 2)][SerializeField] float _climbSpeed = 1f;
    Climbable _currentClimbable;

    [Space(15)]
    [SerializeField] float _interactRange = 1f;
    [SerializeField] Collider _collider;

    LandPlayerState _playerState = LandPlayerState.Default;
    
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
                Debug.Log(direction.y);
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

            default:
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
