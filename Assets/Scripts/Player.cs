using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask slotLayer;
    [SerializeField] private Transform highlight;

    [Header("Attributes")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float tileSize = 1f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 lookDirection = Vector2.down;
    private int tileLayer;
    private int tileUnderPlayer;
    private readonly string nameTileLayer = "Tile";
    private readonly string nameTileUnderPlayer = "TileUnderPlayer";

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        highlight.gameObject.SetActive(false);
        tileLayer = LayerMask.NameToLayer(nameTileLayer);
        tileUnderPlayer = LayerMask.NameToLayer(nameTileUnderPlayer);
    }

    private void Update()
    {
        Move();
        SetAnimationWalking();
        UpdateSelection();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        setTileUnderPlayerLayer(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        setTileLayer(other);
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
            lookDirection = moveInput;
            animator.SetBool("IsWalking", true);
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);
        }
    }

    private void SetAnimationIdle()
    {
        lookDirection = moveInput;
        animator.SetBool("IsWalking", false);
        animator.SetFloat("LastInputX", lookDirection.x);
        animator.SetFloat("LastInputY", lookDirection.y);
    }

    private void UpdateSelection()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            lookDirection,
            tileSize,
            slotLayer
        );

        if (hit.collider != null)
        {
            highlight.gameObject.SetActive(true);
            highlight.position = hit.collider.transform.position;
        }
        else
        {
            highlight.gameObject.SetActive(false);
        }
    }

    private void setTileLayer(Collider2D collider)
    {
        setLayerCollider(collider, tileLayer);
    }

    private void setTileUnderPlayerLayer(Collider2D collider)
    {
        setLayerCollider(collider, tileUnderPlayer);
    }

    private void setLayerCollider(Collider2D collider, int layer)
    {
        if (collider.CompareTag(nameTileLayer))
            collider.gameObject.layer = layer;
    }
}
