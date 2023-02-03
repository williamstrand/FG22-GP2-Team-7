using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] InputHandler _landPlayer;
    [SerializeField] InputHandler _waterPlayer;
    [SerializeField] bool _singlePlayer;

    InputDevice _device1;
    InputDevice _device2;

    void Start()
    {
        StartCoroutine(DevicePairing());
    }

    IEnumerator DevicePairing()
    {
        var action = new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>");
        action.Enable();

        Debug.Log("Press any button on the first device");
        while (_device1 == null)
        {
            if (action.triggered)
            {
                var device = action.activeControl.device;
                if (device is Keyboard or Gamepad)
                {
                    _device1 = device;
                }
            }
            yield return null;
        }
        Debug.Log("Player 1 bound to " + _device1.displayName);
        if (_singlePlayer)
        {
            _landPlayer.Join(_device1);
            yield break;
        }

        Debug.Log("Press any button on the second device");
        while (_device2 == null)
        {
            if (action.triggered)
            {
                var device = action.activeControl.device;
                if (device != _device1 && device is Keyboard or Gamepad)
                {
                    _device2 = device;
                }
            }
            yield return null;
        }
        Debug.Log("Player 2 bound to " + _device2.displayName);

        action.Disable();

        _landPlayer.Join(_device1);
        _waterPlayer.Join(_device2);
    }
}
