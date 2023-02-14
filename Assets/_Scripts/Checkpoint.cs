using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] Transform _respawnPoint;
    [SerializeField] Player _player;

    public enum Player
    {
        Landie,
        Swimmie
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        var characterController = other.gameObject.GetComponent<CharacterController>();
        switch (characterController)
        {
            case LandCharacterController when _player == Player.Landie:
            case WaterCharacterController when _player == Player.Swimmie:
                characterController.SetRespawnPoint(_respawnPoint.position);
                gameObject.SetActive(false);
                break;
        }
    }
}
