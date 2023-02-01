using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WaterCharacterController : CharacterController
{
    const float DiveDepth = 1f;
    
    WaterPlayerState _playerState = WaterPlayerState.Default;
    bool _canDive = true;

    public enum WaterPlayerState
    {
        Default,
        Diving
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void PlayerAction()
    {
        if (!CanDive()) return;

        switch (_playerState)
        {
            case WaterPlayerState.Default:
                Dive();
                break;

            case WaterPlayerState.Diving:
                Resurface();
                break;
        }
        
    }

    void Dive()
    {
        _playerState = WaterPlayerState.Diving;
        StartCoroutine(DiveRoutine());
    }

    void Resurface()
    {
        _playerState = WaterPlayerState.Default;
        StartCoroutine(ResurfaceRoutine());
    }

    IEnumerator DiveRoutine()
    {
        _canDive = false;
        var targetY = _meshRenderer.transform.position.y - DiveDepth;
        var startY = _meshRenderer.transform.position.y;
        
        var lerp = 0f;
        while (lerp < 1)
        {
            lerp += Time.deltaTime;
            var targetPosition = new Vector3(_meshRenderer.transform.position.x, targetY, _meshRenderer.transform.position.z);
            var startPosition = new Vector3(_meshRenderer.transform.position.x, startY, _meshRenderer.transform.position.z);

            _meshRenderer.transform.position = Vector3.Lerp(startPosition, targetPosition, lerp);
            yield return null;
        }

        _canDive = true;
    }

    IEnumerator ResurfaceRoutine()
    {
        _canDive = false;
        var targetY = _meshRenderer.transform.position.y + DiveDepth;
        var startY = _meshRenderer.transform.position.y;

        var lerp = 0f;
        while (lerp < 1)
        {
            lerp += Time.deltaTime;
            var targetPosition = new Vector3(_meshRenderer.transform.position.x, targetY, _meshRenderer.transform.position.z);
            var startPosition = new Vector3(_meshRenderer.transform.position.x, startY, _meshRenderer.transform.position.z);
            
            _meshRenderer.transform.position = Vector3.Lerp(startPosition, targetPosition, lerp);
            yield return null;
        }
        
        _canDive = true;
    }

    bool CanDive()
    {
        return _canDive;
    }
}
