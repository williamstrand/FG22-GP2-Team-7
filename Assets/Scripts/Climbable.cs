using UnityEngine;

public class Climbable : MonoBehaviour
{
    [SerializeField] Transform _topPoint;
    [SerializeField] Transform _bottomPoint;

    Transform _characterTransform;
    float _characterHeight;

    public void StartClimb(CharacterController character)
    {
        _characterTransform = character.transform;
        var a = _topPoint.position - _bottomPoint.position;
        var b = _characterTransform.position - _bottomPoint.position;
        _characterHeight = b.magnitude / a.magnitude;
        _characterTransform.position = Vector3.Lerp(_bottomPoint.position, _topPoint.position, _characterHeight);
    }

    public void StopClimb()
    {
        _characterTransform = null;
    }

    public void Climb(int direction, float speed)
    {
        if (direction == 0) return;

        var dir = Mathf.Sign(direction);
        _characterHeight += dir * speed * Time.deltaTime;
        _characterTransform.position = Vector3.Lerp(_bottomPoint.position, _topPoint.position, _characterHeight);
    }
}
