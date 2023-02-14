using UnityEngine;

public class WaterHazard : MonoBehaviour
{

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player") return;
        if (!other.gameObject.TryGetComponent(out LandCharacterController characterController)) return;
            
        characterController.Respawn();
    }
}
