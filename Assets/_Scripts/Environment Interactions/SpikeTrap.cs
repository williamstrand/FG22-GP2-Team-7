using System;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    /// <summary>
    /// The gameobjects that should be raised or retracted goes here.
    /// </summary>
    [SerializeField] GameObject[] _spikes;
    
    [Header("Raise")]
    [SerializeField] float _raiseHeight = 1.0f;
    [SerializeField] float _raiseSpeed = 1.0f;
    
    [Header("Retract")]
    [SerializeField] float _retractSpeed = 1.0f;
    [SerializeField] float _retractInterval = 1.0f;
    [SerializeField] bool _retractSpikes = true;
    [SerializeField] bool _repeat = false;

    private Vector3[] _originalPositions;
    private Vector3[] _raisedPositions;
    private bool _raised = false;

    void Start()
    {
        _originalPositions = new Vector3[_spikes.Length];
        _raisedPositions = new Vector3[_spikes.Length];
        for (int i = 0; i < _spikes.Length; i++)
        {
            _originalPositions[i] = _spikes[i].transform.position;
            _raisedPositions[i] = _originalPositions[i] + Vector3.up * _raiseHeight;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Coconut"))
        {
            RaiseSpikes();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Coconut") && !_retractSpikes && !_repeat)
        {
            RetractSpikes();
        }
    }

    void RaiseSpikes()
    {
        _raised = true;
        for (int i = 0; i < _spikes.Length; i++)
        {
            _spikes[i].transform.position = Vector3.Lerp(_spikes[i].transform.position, _raisedPositions[i], Time.deltaTime * _raiseSpeed);
        }
        if (_retractSpikes)
        {
            Invoke("RetractSpikes", _retractInterval);
        }
    }

    void RetractSpikes()
    {
        _raised = false;
        for (int i = 0; i < _spikes.Length; i++)
        {
            _spikes[i].transform.position = Vector3.Lerp(_spikes[i].transform.position, _originalPositions[i], Time.deltaTime * _retractSpeed);
        }
        if (_repeat)
        {
            Invoke("RaiseSpikes", _retractInterval);
        }
    }

    void Update()
    {
        if (_raised)
        {
            for (int i = 0; i < _spikes.Length; i++)
            {
                _spikes[i].transform.position = Vector3.Lerp(_spikes[i].transform.position, _raisedPositions[i], Time.deltaTime * _raiseSpeed);
            }
        }
        else
        {
            for (int i = 0; i < _spikes.Length; i++)
            {
                _spikes[i].transform.position = Vector3.Lerp(_spikes[i].transform.position, _originalPositions[i], Time.deltaTime * _retractSpeed);
            }
        }
    }
}
