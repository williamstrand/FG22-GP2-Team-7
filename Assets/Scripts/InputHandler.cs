using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Action OnJump;
    public Action OnInteract;
    public Action OnPlayerAction;

    public Vector2 MoveInput { get; private set; }
    public bool InteractButtonHeld { get; private set; }


    Controls _controls;

    void Awake()
    {
        _controls = new Controls();
        _controls.Player.Jump.performed += _ => OnJump?.Invoke();
        _controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.Player.Interact.performed += _ => OnInteract?.Invoke();
        _controls.Player.Interact.performed += _ => InteractButtonHeld = true;
        _controls.Player.Interact.canceled += _ => InteractButtonHeld = false;
        _controls.Player.PlayerAction.performed += _ => OnPlayerAction?.Invoke();
        _controls.Enable();
    }
}
