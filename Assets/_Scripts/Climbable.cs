using UnityEngine;

public class Climbable : MonoBehaviour, IInteractable
{
    [SerializeField] Transform _topPoint;
    [SerializeField] Transform _bottomPoint;

    Transform _characterTransform;
    float _characterHeight;

    /// <summary>
    /// Start climbing.
    /// </summary>
    /// <param name="character">the character that will start climbing.</param>
    public void StartClimb(CharacterController character)
    {
        _characterTransform = character.transform;
        var botToTop = _topPoint.position - _bottomPoint.position;
        var botToCharacter = _characterTransform.position - _bottomPoint.position;
        _characterHeight = botToCharacter.magnitude / botToTop.magnitude;
        _characterTransform.position = GetPositionOnClimbable(_characterHeight);

        var dir = -(character.transform.position - transform.position).normalized;

        var rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        character.transform.rotation = rotation;
    }

    /// <summary>
    /// Stop climbing.
    /// </summary>
    public void StopClimb()
    {
        _characterTransform = null;
    }

    /// <summary>
    /// Climb in a direction.
    /// </summary>
    /// <param name="direction">the direction. 1 is up, -1 is down.</param>
    /// <param name="speed">the speed of the climb.</param>
    public void Climb(int direction, float speed)
    {
        if (direction == 0) return;

        var dir = Mathf.Sign(direction);
        _characterHeight += dir * speed * Time.deltaTime;
        _characterTransform.position = GetPositionOnClimbable(_characterHeight);
    }

    /// <summary>
    /// Gets the correct position on object based on height.
    /// </summary>
    /// <param name="height">the height.</param>
    /// <returns>the position on the climbable object at the correct height.</returns>
    Vector3 GetPositionOnClimbable(float height) => Vector3.Lerp(_bottomPoint.position, _topPoint.position, height);

#if UNITY_EDITOR

    [Header("Gizmos")]
    [SerializeField] bool _drawGizmos = true;
    [SerializeField] Color _gizmoColor = Color.red;

    void OnDrawGizmos()
    {
        if (!_drawGizmos) return;

        Gizmos.color = _gizmoColor;
        var radius = .2f;
        Gizmos.DrawSphere(_bottomPoint.position, radius);
        Gizmos.DrawSphere(_topPoint.position, radius);
        Gizmos.DrawLine(_bottomPoint.position, _topPoint.position);
    }

#endif
}
