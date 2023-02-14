using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] Transform _respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterController>().SetRespawnPoint(_respawnPoint.position);
            gameObject.SetActive(false);
        }
    }
}
