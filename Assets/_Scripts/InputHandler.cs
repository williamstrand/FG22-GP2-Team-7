using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    public static int PlayerIndex { get; private set; }

    public Action OnJump;
    public Action OnInteract;
    public Action OnInteractUp;
    public Action OnPlayerAction;

    public Vector2 MoveInput { get; private set; }
    public bool InteractButtonHeld { get; private set; }

    Controls _controls;
    InputUser _user;

    void Awake()
    {
        PlayerIndex = 0;
        
        _controls = new Controls();
        _controls.Player.Jump.performed += _ => OnJump?.Invoke();

        _controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();

        _controls.Player.Interact.performed += _ => OnInteract?.Invoke();
        _controls.Player.Interact.canceled += _ => OnInteractUp?.Invoke();
        _controls.Player.Interact.performed += _ => InteractButtonHeld = true;
        _controls.Player.Interact.canceled += _ => InteractButtonHeld = false;

        _controls.Player.PlayerAction.performed += _ => OnPlayerAction?.Invoke();
    }

    public void Join(InputDevice device, bool keyboard2 = false)
    {
        _user = InputUser.PerformPairingWithDevice(device);
        _user.AssociateActionsWithUser(_controls);
        if (keyboard2)
        {
            _user.ActivateControlScheme(PlayerIndex == 0 ? "KBM" : "KBM2");
            PlayerIndex++;
        }
        _controls.Enable();
    }
}
