using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDrop : MonoBehaviour
{
    private Transform _pickUpPoint;
    [SerializeField] private Transform _player;
    
    [Header("Pickup/Drop")]
    [SerializeField] private float _pickUpDistance;
    [SerializeField] private float _forceMultiplier;
    [SerializeField] private float _baseForce;
    [SerializeField] private bool _readyToThrow;
    [SerializeField] private bool _itemIsPicked;
    
    [Header("Push/Pull")]
    [SerializeField] private float _pushPullDistance;
    [SerializeField] private float _pushPullMultiplier;
    [SerializeField] private float _pushPullBaseForce;
    [SerializeField] private bool _isPushing;
    [SerializeField] private bool _isPulling;

    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _pickUpPoint = GameObject.Find("PickupPoint").transform;
    }

    private void Update()
    {
        PickupDrop();
        PushPull();
    }

    private void PickupDrop()
    {
        if (Input.GetKey(KeyCode.G) && _itemIsPicked && _readyToThrow)
        {
            _rigidbody.isKinematic = false;
            _forceMultiplier += _baseForce * Time.deltaTime;
        }

        _pickUpDistance = Vector3.Distance(_player.position, transform.position);

        if (_pickUpDistance <= 2)
        {
            if (Input.GetKeyDown(KeyCode.G) && !_itemIsPicked && _pickUpPoint.childCount < 1)
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<BoxCollider>().enabled = false;
                _rigidbody.isKinematic = true;
                transform.position = _pickUpPoint.position;
                transform.parent = GameObject.Find("PickupPoint").transform;

                _itemIsPicked = true;
                _forceMultiplier = 0;
                
            }
        }

        if (Input.GetKeyUp(KeyCode.G) && _itemIsPicked)
        {
            _readyToThrow = true;

            if (_forceMultiplier > 10)
            {
                _rigidbody.AddForce(_player.transform.TransformDirection(0, 0, 1) * _forceMultiplier);
                transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<BoxCollider>().enabled = true;
                _itemIsPicked = false;
                _forceMultiplier = 0;
                _readyToThrow = false;
            }

            _forceMultiplier = 0;
        }
    }
    
    private void PushPull()
    {
        _pushPullDistance = Vector3.Distance(_player.position, transform.position);

        if (_pushPullDistance <= 2)
        {
            if (Input.GetKey(KeyCode.H) && !_isPushing && !_isPulling)
            {
                _isPulling = true;
                _pushPullMultiplier = 0;
            }

            if (Input.GetKey(KeyCode.J) && !_isPushing && !_isPulling)
            {
                _isPushing = true;
                _pushPullMultiplier = 0;
            }
        }

        if (_isPulling)
        {
            _pushPullMultiplier -= _pushPullBaseForce * Time.deltaTime;
        }

        if (_isPushing)
        {
            _pushPullMultiplier += _pushPullBaseForce * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.H) && _isPulling)
        {
            _rigidbody.AddForce(-_player.transform.TransformDirection(0,0,-1) * _pushPullMultiplier, ForceMode.Impulse);
            _isPulling = false;
            _pushPullMultiplier = 0;
        }

        if (Input.GetKeyUp(KeyCode.J) && _isPushing)
        {
            _rigidbody.AddForce(_player.transform.TransformDirection(0,0,1) * _pushPullMultiplier, ForceMode.Impulse);
            _isPushing = false;
            _pushPullMultiplier = 0;
        }
    }
}
