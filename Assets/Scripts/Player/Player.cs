using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask slotLayer;
    [SerializeField] private Transform highlight;
    [SerializeField] private GameObject[] turrets;

    [Header("Attributes")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float ease = 0.2f;
    [SerializeField] private TowerRequestManager towerRequestManager;
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private Vector2 lookDirection = Vector2.down;
    private int tileLayer;
    private int tileUnderPlayer;
    private readonly string nameTileLayer = "Tile";
    private readonly string nameTileUnderPlayer = "TileUnderPlayer";
    private Vector2 velocityRef;

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

    public void ReadLookInput(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            SetAnimationIdle();
            return;
        }
        SetAnimationIdle();
        lookDirection = context.ReadValue<Vector2>();
    }

    private void Move()
    {
        //Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
        //Debug.Log("Movement:" + movement * speed);
        // rb.linearVelocity = movement * speed;

        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
        Vector2 targetVelocity = new Vector2(movement.x, movement.y) * speed;
        // SmoothDamp usa 'ease' como smoothTime (tempo aproximado para alcançar o alvo)
        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref velocityRef, ease);
        //Debug.Log("Velocity:" + rb.linearVelocity);
    }

    public void ReadActionInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SpawnTurret();
        }
    }

    private void SpawnTurret()
    {
        GameObject turretToSpawn = turrets[0];
        if (highlight.gameObject.activeSelf)
        {
            towerRequestManager.RequestTowerBuy("Penguin");
            if (towerRequestManager.isTowerPriced)
            {
                Instantiate(turretToSpawn, highlight.position, Quaternion.identity);
                towerRequestManager.isTowerPriced = false;
            }
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
        if (moveInput.magnitude > 0)
        {
            lookDirection = moveInput;
        }
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
