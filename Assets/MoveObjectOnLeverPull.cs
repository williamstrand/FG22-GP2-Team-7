using System.Collections;
using UnityEngine;

public class MoveObjectOnLeverPull : MonoBehaviour
{
    [SerializeField] private LeverPull _leverPull;
    [SerializeField] private float _moveDistance = 1f;
    [SerializeField] private float _smoothTime = 0.2f;
    [SerializeField] private bool _moveDown = true;

    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private Vector3 _velocity;
    private bool _isMoving;

    private Coroutine _moveCoroutine;

    private void Start()
    {
        _initialPosition = transform.position;
        _leverPull.onPull.AddListener(OnLeverPull);
    }

    private void OnLeverPull()
    {
        if (_isMoving) return;

        if (_moveDown)
        {
            _targetPosition = transform.position - transform.up * _moveDistance;
            _moveDown = false;
        }
        else
        {
            _targetPosition = transform.position + transform.up * _moveDistance;
            _moveDown = true;
        }

        _moveCoroutine = StartCoroutine(MoveObjectCoroutine());
    }

    private IEnumerator MoveObjectCoroutine()
    {
        _isMoving = true;

        while (Vector3.Distance(transform.position, _targetPosition) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, _smoothTime);
            yield return null;
        }

        _isMoving = false;
    }
}