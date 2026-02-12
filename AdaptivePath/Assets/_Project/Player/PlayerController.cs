using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    private Vector2 moveInput;

    [SerializeField] private float moveSpeed = 5f;

    private PlayerInputActions InputActions;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        InputActions = new PlayerInputActions();

        InputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        InputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        InputActions.Player.Enable();
    }

    private void OnDisable()
    {
        InputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    private void move()
    {
        
    }


}
