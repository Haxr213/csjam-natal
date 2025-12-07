using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector2 moveInput;
    //[SerializeField] private Vector2 lastMoveInput;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        SetAnimationWalking();
    }

    public void ReadMoveInput(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            SetAnimationIdle();
        }

        moveInput = context.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
        rb.linearVelocity = movement * speed;
    }

    public void ReadActionInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Action performed!");
        }
    }
    private void SetAnimationWalking()
    {
        bool isWalking = moveInput.magnitude > 0;
        if (isWalking)
        {
            animator.SetBool("IsWalking", true);
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);
        }
    }

    private void SetAnimationIdle()
    {
        //lastMoveInput = moveInput;
        animator.SetBool("IsWalking", false);
        animator.SetFloat("LastInputX", moveInput.x);
        animator.SetFloat("LastInputY", moveInput.y);
    }
}
