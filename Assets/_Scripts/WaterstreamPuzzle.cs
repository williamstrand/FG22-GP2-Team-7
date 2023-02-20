using System.Collections;
using UnityEngine;

public class WaterstreamPuzzle : MonoBehaviour
{
    [SerializeField] DropOffZone _dropOffZone;

    [SerializeField] Vector2Int _lever1Positions;
    [SerializeField] Vector2Int _lever2Positions;
    [SerializeField] Vector2Int _lever3Positions;

    Pushable _lane1Pushable;
    int _lane1Position;
    [SerializeField] Transform[] _lane1Positions;
    bool _lane1Ready;

    Pushable _lane2Pushable;
    int _lane2Position;
    [SerializeField] Transform[] _lane2Positions;
    bool _lane2Ready;

    [SerializeField] float _moveSpeed = 1;

    [Header("Testing")]
    [SerializeField] bool _lever1;
    [SerializeField] bool _lever2;
    [SerializeField] bool _lever3;

    void OnEnable()
    {
        _dropOffZone.OnDropOff += OnDropOff;
    }

    void OnDisable()
    {
        _dropOffZone.OnDropOff -= OnDropOff;
    }

    void OnDropOff(Pushable pushable)
    {
        pushable.GetComponent<Rigidbody>().isKinematic = true;
        pushable.GetComponent<Collider>().enabled = false;
        pushable.enabled = false;

        if (_lane1Pushable == null)
        {
            _lane1Position = 0;
            _lane1Pushable = pushable;
            StartCoroutine(MoveToStart(1));
        }
        else if (_lane2Pushable == null)
        {
            if (_lane1Pushable == pushable) return;

            _lane1Position = 0;
            _lane2Pushable = pushable;
            StartCoroutine(MoveToStart(2));
        }
    }

    public void OnLever1()
    {
        _lever1 = !_lever1;
    }

    public void OnLever2()
    {
        _lever2 = !_lever2;
    }

    public void OnLever3()
    {
        _lever3 = !_lever3;
    }



    void Update()
    {
        if (_lever1)
        {
            if (_lane1Position == _lever1Positions.x)
            {
                if (!_lane1Ready) return;
                MoveLane1();
            }
        }
        else
        {
            if (_lane2Position == _lever1Positions.y)
            {
                if (!_lane2Ready) return;
                MoveLane2();
            }
        }

        if (_lever2)
        {
            if (_lane1Position == _lever2Positions.x)
            {
                if (!_lane1Ready) return;
                MoveLane1();
            }
        }
        else
        {
            if (_lane2Position == _lever2Positions.y)
            {
                if (!_lane2Ready) return;
                MoveLane2();
            }
        }

        if (_lever3)
        {
            if (_lane1Position == _lever3Positions.x)
            {
                if (!_lane1Ready) return;
                MoveLane1();
            }
        }
        else
        {
            if (_lane2Position == _lever3Positions.y)
            {
                if (!_lane2Ready) return;
                MoveLane2();
            }
        }
    }

    void MoveLane2()
    {
        StartCoroutine(MoveLane(2, _lane2Position, _lane2Position + 1));
        _lane2Position++;
    }

    void MoveLane1()
    {
        StartCoroutine(MoveLane(1, _lane1Position, _lane1Position + 1));
        _lane1Position++;
    }

    IEnumerator MoveLane(int lane, int startPos, int endPos)
    {
        if (lane == 1)
        {
            _lane1Ready = false;
        }
        else if (lane == 2)
        {
            _lane2Ready = false;
        }

        var lerp = 0f;

        while (lerp < 1)
        {
            lerp += Time.deltaTime * _moveSpeed;
            if (lane == 1)
            {
                _lane1Pushable.transform.position = Vector3.Lerp(_lane1Positions[startPos].position, _lane1Positions[endPos].position, lerp);
            }
            else
            {
                _lane2Pushable.transform.position = Vector3.Lerp(_lane2Positions[startPos].position, _lane2Positions[endPos].position, lerp);
            }
            yield return null;
        }

        if (lane == 1)
        {
            _lane1Ready = true;

            if (_lane1Position + 1 >= _lane1Positions.Length)
            {
                _lane1Pushable.enabled = false;
                _lane1Pushable.GetComponent<Rigidbody>().isKinematic = false;
                _lane1Pushable.GetComponent<Collider>().enabled = true;
                _lane1Pushable = null;
            }
        }
        else if (lane == 2)
        {
            _lane2Ready = true;

            if (_lane2Position + 1 >= _lane2Positions.Length)
            {
                _lane2Pushable.enabled = false;
                _lane2Pushable.GetComponent<Rigidbody>().isKinematic = false;
                _lane2Pushable.GetComponent<Collider>().enabled = true;
                _lane2Pushable = null;
            }
        }
    }

    IEnumerator MoveToStart(int lane)
    {
        Vector3 start;
        if (lane == 1)
        {
            _lane1Ready = false;
            start = _lane1Pushable.transform.position;
        }
        else
        {
            _lane2Ready = false;
            start = _lane2Pushable.transform.position;
        }

        var lerp = 0f;

        while (lerp < 1)
        {
            lerp += Time.deltaTime;
            if (lane == 1)
            {
                _lane1Pushable.transform.position = Vector3.Lerp(start, _lane1Positions[0].position, lerp);
            }
            else
            {
                _lane2Pushable.transform.position = Vector3.Lerp(start, _lane2Positions[0].position, lerp);
            }
            yield return null;
        }

        if (lane == 1)
        {
            _lane1Ready = true;
        }
        else if (lane == 2)
        {
            _lane2Ready = true;
        }
    }

}
