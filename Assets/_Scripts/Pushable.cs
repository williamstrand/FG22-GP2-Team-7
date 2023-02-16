using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Pushable : MonoBehaviour, IInteractable
{
    Transform _player;
    [SerializeField] float _pushOffset;
    public bool IsPushed => _player != null;
    bool _canPush = true;

    /// <summary>
    /// Starts pushing the object.
    /// </summary>
    /// <param name="player">the player that will push.</param>
    public void StartPush(Transform player)
    {
        var points = new Vector3[]
        {
            transform.position + transform.forward * _pushOffset,
            transform.position + -transform.forward * _pushOffset,
            transform.position + transform.right * _pushOffset,
            transform.position + -transform.right * _pushOffset
        };

        var closest = points[0];
        foreach (var point in points)
        {
            if (Vector3.Distance(point, player.position) < Vector3.Distance(closest, player.position))
            {
                closest = point;
            }
        }

        player.GetComponent<Collider>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.transform.position = new Vector3(closest.x, player.transform.position.y, closest.z);
        player.SetParent(transform);

        _player = player;
    }

    /// <summary>
    /// Stops pushing.
    /// </summary>
    public void StopPush()
    {
        _player.GetComponent<Collider>().enabled = true;
        _player.GetComponent<Rigidbody>().isKinematic = false;

        _player.SetParent(null);
        _player = null;
    }

    /// <summary>
    /// Pushes forward.
    /// </summary>
    /// <param name="speed">the speed to push at.</param>
    public void Push(float speed)
    {
        if(!_canPush) return;

        var direction = new Vector3(_player.position.x, 0, _player.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
        direction.Normalize();

        transform.position += speed * Time.deltaTime * -direction;
    }

    /// <summary>
    /// Turns the object in a direction.
    /// </summary>
    /// <param name="direction">the direction.</param>
    public void Turn(float direction)
    {
        if (!_canPush) return;

        var dir = Mathf.Sign(direction);
        transform.Rotate(0, dir * 90 * Time.deltaTime, 0);
    }

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Player")) return;

        if (col.gameObject.TryGetComponent<LandCharacterController>(out var character))
        {
            _canPush = false;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (!col.gameObject.CompareTag("Player")) return;

        if (col.gameObject.TryGetComponent<LandCharacterController>(out var character))
        {
            _canPush = true;
        }
    }
}