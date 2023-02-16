using System.Collections;
using UnityEngine;

public class WaterstreamPuzzle : MonoBehaviour
{
    [SerializeField] DropOffZone _dropOffZone;

    Pushable _lane1Pushable;
    int _lane1Position;
    [SerializeField] Transform[] _lane1Positions;
    bool _lane1Ready;

    Pushable _lane2Pushable;
    int _lane2Position;
    [SerializeField] Transform[] _lane2Positions;
    bool _lane2Ready;

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
        if (_lane1Pushable == null)
        {
            _lane1Pushable = pushable;
            StartCoroutine(MoveToStart(1));
        }
        else if (_lane2Pushable == null)
        {
            _lane2Pushable = pushable;
            StartCoroutine(MoveToStart(2));
        }
    }

    void OnLever1()
    {
        if (_lane1Position == 1 - 1)
        {
            if (!_lane1Ready) return;
            StartCoroutine(MoveLane(1, _lane1Position, _lane1Position + 1));
            _lane1Position++;
        }

        if (_lane2Position == 2 - 1)
        {
            if (!_lane2Ready) return;
            StartCoroutine(MoveLane(2, _lane2Position, _lane2Position + 1));
            _lane2Position++;
        }
    }

    void OnLever2()
    {
        if (_lane1Position == 2 - 1)
        {
            if (!_lane1Ready) return;
            StartCoroutine(MoveLane(1, _lane1Position, _lane1Position + 1));
            _lane1Position++;
        }

        if (_lane2Position == 3 - 1)
        {
            if (!_lane2Ready) return;
            StartCoroutine(MoveLane(2, _lane2Position, _lane2Position + 1));
            _lane2Position++;
        }
    }

    void OnLever3()
    {
        if (_lane1Position == 3 - 1)
        {
            if (!_lane1Ready) return;
            StartCoroutine(MoveLane(1, _lane1Position, _lane1Position + 1));
            _lane1Position++;
        }

        if (_lane2Position == 1 - 1)
        {
            if (!_lane2Ready) return;
            StartCoroutine(MoveLane(2, _lane2Position, _lane2Position + 1));
            _lane2Position++;
        }
    }

    [Header("Testing")]
    [SerializeField] bool _lever1;
    [SerializeField] bool _lever2;
    [SerializeField] bool _lever3;

    void Update()
    {
        if (_lever1)
        {
            OnLever1();
        }

        if (_lever2)
        {
            OnLever2();
        }

        if (_lever3)
        {
            OnLever3();
        }
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
            lerp += Time.deltaTime;
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
                _lane1Pushable = null;
            }
        }
        else if (lane == 2)
        {
            _lane2Ready = true;

            if (_lane2Position + 1 >= _lane2Positions.Length)
            {
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
