using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] InputHandler _landPlayer;
    [SerializeField] InputHandler _waterPlayer;
    [SerializeField] Player _player;

    public enum Player
    {
        Both,
        Landie,
        Swimmie
    }

    
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
        switch (_player)
        {
            case Player.Both:
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
                break;

            case Player.Landie:
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

                action.Disable();

                _landPlayer.Join(_device1);
                break;

            case Player.Swimmie:
                Debug.Log("Press any button on the first device");
                while (_device2 == null)
                {
                    if (action.triggered)
                    {
                        var device = action.activeControl.device;
                        if (device is Keyboard or Gamepad)
                        {
                            _device2 = device;
                        }
                    }
                    yield return null;
                }
                Debug.Log("Player 2 bound to " + _device2.displayName);

                action.Disable();

                _waterPlayer.Join(_device2);
                break;

        }
    }
}
