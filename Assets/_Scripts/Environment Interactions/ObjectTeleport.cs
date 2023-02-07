using UnityEngine;

public class ObjectTeleport : MonoBehaviour
{
    [SerializeField] Transform _targetObject, _teleportPosition;
    [SerializeField] TriggerController _triggerController;

  

    private void Update()
    {
        if (_triggerController.isPressed)
        {
            _targetObject.position = _teleportPosition.position;
        }
    }
}
