using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Action OnJump;
    public Action<Vector2> OnMove;
    public Action OnInteract;
    public Action OnPlayerAction;

    Vector2 _moveInput;

    Controls _controls;

    void Awake()
    {
        _controls = new Controls();
        _controls.Player.Jump.performed += _ => OnJump?.Invoke();
        _controls.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.Player.Interact.performed += _ => OnInteract?.Invoke();
        _controls.Player.PlayerAction.performed += _ => OnPlayerAction?.Invoke();
        _controls.Enable();
    }

    void Update()
    {
        OnMove?.Invoke(_moveInput);
    }
}
