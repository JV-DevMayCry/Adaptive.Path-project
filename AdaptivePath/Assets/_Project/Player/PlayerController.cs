using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    private Vector2 moveInput;
    private Animator animator;
    private PlayerInputActions InputActions;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform cameraTransform;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

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
        move();
    }

    private void move()
    {

        animator.SetFloat("Speed", moveInput.magnitude, 0.1f, Time.deltaTime);

        float cameraY = cameraTransform.eulerAngles.y;
        Quaternion cameraRotation = Quaternion.Euler(0f, cameraY, 0f);

        Vector3 direction = cameraRotation * new Vector3(moveInput.x, 0f, moveInput.y);
        direction.Normalize();

        Vector3 movement = direction * moveSpeed;

        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);

        if (moveInput.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        }
    }


}
