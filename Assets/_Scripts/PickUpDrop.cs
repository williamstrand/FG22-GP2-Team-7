using UnityEngine;

public class PickUpDrop : MonoBehaviour, IInteractable
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
        PushPull();
    }

    public void ChargeThrow()
    {
        if (_itemIsPicked && _readyToThrow)
        {
            _forceMultiplier += _baseForce * Time.deltaTime;
        }
    }

    public void Pickup(Transform player)
    {
        _pickUpDistance = Vector3.Distance(_player.position, transform.position);

        if (_pickUpDistance <= 2)
        {
            if (!_itemIsPicked && _pickUpPoint.childCount < 1)
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Collider>().enabled = false;
                _rigidbody.isKinematic = true;
                transform.position = _pickUpPoint.position;
                transform.parent = GameObject.Find("PickupPoint").transform;

                _itemIsPicked = true;
                _forceMultiplier = 0;
                _readyToThrow = true;

            }
        }
    }

    public bool Throw()
    {
        if (_itemIsPicked)
        {

            if (_forceMultiplier > 1)
            {
                _rigidbody.AddForce(_player.transform.TransformDirection(0, 0, 1) * _forceMultiplier);
                transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Collider>().enabled = true;
                _itemIsPicked = false;
                _forceMultiplier = 0;
                _rigidbody.isKinematic = false;
                _readyToThrow = false;
                return true;
            }
            _forceMultiplier = 0;
            return false;
        }

        return false;
    }

    public void Drop()
    {
        transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = true;
        _itemIsPicked = false;
        _rigidbody.isKinematic = false;
        _readyToThrow = false;
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
            _rigidbody.AddForce(-_player.transform.TransformDirection(0, 0, -1) * _pushPullMultiplier, ForceMode.Impulse);
            _isPulling = false;
            _pushPullMultiplier = 0;
        }

        if (Input.GetKeyUp(KeyCode.J) && _isPushing)
        {
            _rigidbody.AddForce(_player.transform.TransformDirection(0, 0, 1) * _pushPullMultiplier, ForceMode.Impulse);
            _isPushing = false;
            _pushPullMultiplier = 0;
        }
    }
}
