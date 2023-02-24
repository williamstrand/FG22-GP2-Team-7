using UnityEngine;

public class LandCharacterController : CharacterController
{
    [Header("Climbing")]
    [Range(0, 2)][SerializeField] float _climbSpeed = 1f;
    Climbable _currentClimbable;
    [SerializeField] AudioSource _climbAudioSource;
    [SerializeField] float _climbVolume = 1;

    [Header("Catapult")]
    [SerializeField] float _catapultAimSpeed = 1f;
    Catapult _catapult;

    [Header("Pickup")]
    [SerializeField] Transform _pickupPoint;
    [SerializeField] AudioSource _pickupAudioSource;
    [SerializeField] float _pickupVolume = 1;
    [SerializeField] AudioSource _dropAudioSource;
    [SerializeField] float _dropVolume = 1;

    [Header("Torch")]
    [SerializeField] TorchIgnition _torch;
    [SerializeField] GameObject _torchMesh;
    bool _torchOn = false;

    [SerializeField] AudioSource _landingAudioSource;
    [SerializeField] float _landingVolume = 1;

    LandPlayerState _playerState = LandPlayerState.Default;
    PickUpDrop _heldItem;

    public enum LandPlayerState
    {
        Default,
        Climbing,
        Catapult
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _torch.OnTorchLit += OnTorchLit;
        _torch.OnTorchPutOut += OnTorchPutOut;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _torch.OnTorchLit -= OnTorchLit;
        _torch.OnTorchPutOut -= OnTorchPutOut;
    }

    void OnTorchPutOut()
    {
        _torchOn = false;
        _torchMesh.SetActive(false);
        _animator.SetBool("TorchOn", false);
    }

    void OnTorchLit()
    {
        _torchOn = true;
        _torchMesh.SetActive(true);
        _animator.SetBool("TorchOn", true);
        _animator.SetTrigger("TorchLit");
    }

    protected override void FixedUpdate()
    {
        switch (_playerState)
        {
            case LandPlayerState.Default:
                base.FixedUpdate();
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

    protected override void Jump()
    {
        switch (_playerState)
        {
            case LandPlayerState.Default:
                base.Jump();
                break;

            case LandPlayerState.Climbing:
                _currentClimbable.StopClimb();
                _currentClimbable = null;
                StopClimb();
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
                _animator.SetFloat("ClimbDirection", direction.y);

                PlaySound(_climbAudioSource, _climbVolume, direction.y != 0);
                break;

            case LandPlayerState.Catapult:
                _catapult.SetAim(_catapult.AimAngle + direction.y * _catapultAimSpeed);
                _catapult.SetForce(_catapult.Force + direction.x * _catapultAimSpeed * .1f);
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
                    return;

                case Climbable climbable:
                    if (_heldItem) break;
                    climbable.StartClimb(this);
                    _currentClimbable = climbable;
                    StartClimb();
                    _footstepAudioSource.Stop();
                    return;

                case Catapult when _playerState == LandPlayerState.Catapult:
                    _catapult.ExitCatapult(this);
                    _catapult = null;
                    _playerState = LandPlayerState.Default;
                    return;

                case Catapult catapult when !_heldItem:
                    catapult.EnterCatapult(this);
                    _playerState = LandPlayerState.Catapult;
                    _catapult = catapult;
                    return;

                case Catapult catapult when _heldItem:
                    PlaySound(_dropAudioSource, _dropVolume);
                    _heldItem.Drop();
                    catapult.LoadCoconut(_heldItem);
                    _animator.SetBool("HoldingItem", false);
                    _heldItem = null;
                    return;

                case PickUpDrop pickUpDrop:
                    if (_heldItem) break;
                    pickUpDrop.GetComponentInChildren<UIInteraction>().enabled = false;
                    pickUpDrop.Pickup(_pickupPoint);
                    _heldItem = pickUpDrop;
                    PlaySound(_pickupAudioSource, _pickupVolume);
                    _animator.SetBool("HoldingItem", true);
                    _animator.SetTrigger("Pickup");
                    return;

                case LeverPull leverPull:
                    _animator.SetTrigger("Hit");
                    leverPull.Pull();
                    break;
            }
        }

        if (!_heldItem) return;
        PlaySound(_dropAudioSource, _dropVolume);
        _heldItem.GetComponentInChildren<UIInteraction>().enabled = true;
        _heldItem.Drop();
        _heldItem = null;
        _animator.SetBool("HoldingItem", false);
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
        _animator.SetBool("Climb", true);
    }

    /// <summary>
    /// Stop climbing.
    /// </summary>
    void StopClimb()
    {
        _playerState = LandPlayerState.Default;
        _collider.enabled = true;
        _rb.velocity = Vector3.zero;
        _climbAudioSource.Stop();
        _animator.SetBool("Climb", false);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            Respawn();
        }

        if (!Physics.Raycast(transform.position, Vector3.down, out var hit, 1, _groundLayer)) return;
        Debug.Log(hit);
        if (hit.collider == null) return;

        if (hit.collider.gameObject != other.gameObject) return;
        PlaySound(_landingAudioSource, _landingVolume);
    }
}
