using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WaterCharacterController : CharacterController
{
    const float DiveDepth = 1f;
    
    WaterPlayerState _playerState = WaterPlayerState.Default;
    bool _canDive = true;

    [SerializeField] float _diveSpeed = 1;

    public enum WaterPlayerState
    {
        Default,
        Diving
    }

    protected override void FixedUpdate()
    {
        switch (_playerState)
        {
            case WaterPlayerState.Default:
                base.FixedUpdate();
                break;

            case WaterPlayerState.Diving:
                break;
        }
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

    void Dive()
    {
        _rb.velocity = Vector3.zero;
        _playerState = WaterPlayerState.Diving;
        StartCoroutine(DiveRoutine());
        Physics.IgnoreLayerCollision(gameObject.layer, 6, true);
    }

    void Resurface()
    {
        _rb.SweepTest(Vector3.up, out var hitInfo, DiveDepth);
        if (hitInfo.collider) return;
        
        _rb.velocity = Vector3.zero;
        _playerState = WaterPlayerState.Default;
        StartCoroutine(ResurfaceRoutine());
        Physics.IgnoreLayerCollision(gameObject.layer, 6, false);
    }

    IEnumerator DiveRoutine()
    {
        _canDive = false;
        var targetY = transform.position.y - DiveDepth;
        var startY = transform.position.y;
        
        var lerp = 0f;
        while (lerp < 1)
        {
            lerp += Time.deltaTime * _diveSpeed;
            var targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            var startPosition = new Vector3(transform.position.x, startY, transform.position.z);

            transform.position = Vector3.Lerp(startPosition, targetPosition, lerp);
            yield return null;
        }

        _canDive = true;
    }

    IEnumerator ResurfaceRoutine()
    {
        _canDive = false;
        var targetY = transform.position.y + DiveDepth;
        var startY = transform.position.y;

        var lerp = 0f;
        while (lerp < 1)
        {
            lerp += Time.deltaTime * _diveSpeed;
            var targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            var startPosition = new Vector3(transform.position.x, startY, transform.position.z);

            transform.position = Vector3.Lerp(startPosition, targetPosition, lerp);
            yield return null;
        }
        
        _canDive = true;
    }

    bool CanDive()
    {
        return _canDive;
    }
}
