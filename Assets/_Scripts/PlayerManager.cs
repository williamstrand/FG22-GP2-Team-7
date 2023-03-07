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
                yield return BindBothPlayers(action);
                break;

            case Player.Landie:
                yield return BindPlayer1(action);
                break;

            case Player.Swimmie:
                yield return BindPlayer2(action);
                break;
        }
        action.Disable();
    }

    IEnumerator BindBothPlayers(InputAction action)
    {
        var dualKeyboard = false;
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

                while (action.ReadValue<float>() > 0)
                {
                    yield return null;
                }
            }
            yield return null;
        }
        Debug.Log("Player 1 bound to " + _device1.displayName);

        Debug.Log("Press any button on the second device");
        while (_device2 == null)
        {
            if (action.triggered)
            {
                var device = action.activeControl.device;
                if (device == _device1 && device is Keyboard)
                {
                    dualKeyboard = true;
                    _device2 = device;
                }
                else if (device != _device1 && device is Keyboard or Gamepad)
                {
                    _device2 = device;
                }

                while (action.ReadValue<float>() > 0)
                {
                    yield return null;
                }
            }
            yield return null;
        }
        Debug.Log("Player 2 bound to " + _device2.displayName);

        _waterPlayer.Join(_device2, dualKeyboard);
        _landPlayer.Join(_device1, dualKeyboard);
    }

    IEnumerator BindPlayer1(InputAction action)
    {
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

                while (action.ReadValue<float>() > 0)
                {
                    yield return null;
                }
            }
            yield return null;
        }
        Debug.Log("Player 1 bound to " + _device1.displayName);

        _landPlayer.Join(_device1);
    }

    IEnumerator BindPlayer2(InputAction action)
    {
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

                while (action.ReadValue<float>() > 0)
                {
                    yield return null;
                }
            }
            yield return null;
        }
        Debug.Log("Player 2 bound to " + _device2.displayName);

        _waterPlayer.Join(_device2);
    }
}
