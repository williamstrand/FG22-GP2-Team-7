using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Action OnJump;
    public Action<Vector2> OnMove;
    public Action OnInteract;

    Vector2 _moveInput;

    Controls _controls;

    void Awake()
    {
        _controls = new Controls();
        _controls.Player.Jump.performed += ctx => OnJump?.Invoke();
        _controls.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.Player.Interact.performed += ctx => OnInteract?.Invoke();
        _controls.Enable();
    }

    void Update()
    {
        OnMove?.Invoke(_moveInput);
    }
}
