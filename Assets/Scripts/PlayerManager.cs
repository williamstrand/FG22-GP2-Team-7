using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] InputHandler _landPlayer;
    [SerializeField] InputHandler _waterPlayer;

    void Start()
    {
        StartCoroutine(DevicePairing());
    }

    IEnumerator DevicePairing()
    {
        InputDevice device1 = null;

        var action = new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>");
        action.Enable();

        while (device1 == null)
        {
            if (action.triggered)
            {
                var device = action.activeControl.device;
                if (device is Keyboard or Gamepad)
                {
                    device1 = device;
                }
            }
            yield return null;
        }

        InputDevice device2 = null;
        while (device2 == null)
        {
            if (action.triggered)
            {
                var device = action.activeControl.device;
                if (device != device1 && device is Keyboard or Gamepad)
                {
                    device2 = device;
                }
            }
            yield return null;
        }
        action.Disable();

        _landPlayer.Join(device1);
        _waterPlayer.Join(device2);
    }
}
