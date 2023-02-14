using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [Header("Platform Positions")]
    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _endPoint;
    
    [SerializeField] float _speed = 1.0f;
    [SerializeField] GameObject _triggerObject;
    [SerializeField] bool _shouldReturn = false;
    
    private TriggerController _triggerController;
    private Vector3 _currentTarget;
    private float _startTime;
    private float _journeyLength;

    private void Start()
    {
        if (_triggerObject != null)
        {
            _triggerController = _triggerObject.GetComponent<TriggerController>();
            if (_triggerController == null)
            {
                Debug.LogError("The trigger object does not have a TriggerController script attached.");
            }
        }
        else
        {
            Debug.LogError("The trigger object has not been set in the PlatformMover script.");
        }

        _currentTarget = _endPoint.position;
        _startTime = Time.time;
        _journeyLength = Vector3.Distance(_startPoint.position, _endPoint.position);
    }

    private void Update()
    {
        if (_triggerController != null && _triggerController.isPressed)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        float distCovered = (Time.time - _startTime) * _speed;
        float fracJourney = distCovered / _journeyLength;
        transform.position = Vector3.Lerp(_startPoint.position, _currentTarget, fracJourney);

       if (transform.position == _startPoint.position)
        {
            _currentTarget = _endPoint.position;
            _startTime = Time.time;
            _journeyLength = Vector3.Distance(_startPoint.position, _endPoint.position);
        }
    }
}
