using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask slotLayer;
    [SerializeField] private Transform highlight;
    [SerializeField] private GameObject[] turrets;
    private TowerSlot currentSlot;

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

    private PlayerManager playerManager;

    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        //playerInput = this.GetComponent<PlayerInput>();
        playerManager = this.GetComponent<PlayerManager>();
    }
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
        if (!highlight.gameObject.activeSelf) return;
        if (currentSlot == null) return;
        if (context.performed)
        {
            if (currentSlot.isOccupied)
            {
                Debug.Log("Slot já ocupado.");
                playerInput.SwitchCurrentActionMap("Remove");
                playerManager.SetActiveRemoveTurretUI();
                playerManager.SetActiveButtonUI();
                playerManager.setInactiveTowersUI();
                return;
            }
            playerInput.SwitchCurrentActionMap("Tower");
            playerManager.SetActiveTowersUI();
            playerManager.SetActiveButtonUI();
            playerManager.SetInactiveRemoveTurretUI();
        }
    }

    public void ReadCancelInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerInput.SwitchCurrentActionMap("Player");
            playerManager.setInactiveTowersUI();
            playerManager.SetInactiveRemoveTurretUI();
            playerManager.SetInactiveButtonUI();
        }
    }

    public void ReadRemoveTurretInput(InputAction.CallbackContext context)
    {
        if (!highlight.gameObject.activeSelf) return;
        if (currentSlot == null) return;

        if (context.performed)
        {
            if (!currentSlot.isOccupied)
            {
                Debug.Log("Slot não ocupado.");
                return;
            }

            Tower tower = currentSlot.GetComponentInChildren<Tower>();
            if (tower != null)
            {
                //towerRequestManager.RequestTowerSell(tower.towerType);
                Destroy(tower.gameObject);
                currentSlot.Free();
            }
            playerInput.SwitchCurrentActionMap("Player");
            playerManager.setInactiveTowersUI();
            playerManager.SetInactiveRemoveTurretUI();
            playerManager.SetInactiveButtonUI();
        }
    }

    public void ReadCancelRemoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerInput.SwitchCurrentActionMap("Player");
            playerManager.setInactiveTowersUI();
            playerManager.SetInactiveRemoveTurretUI();
            playerManager.SetInactiveButtonUI();
        }
    }

    public void ReadSpawnTurretPenguinInput(InputAction.CallbackContext context)
    {
        string turretType = "Penguin";
        if (context.performed)
        {
            SpawnTurret(turretType);
            playerInput.SwitchCurrentActionMap("Player");
            playerManager.setInactiveTowersUI();
            playerManager.SetInactiveRemoveTurretUI();
            playerManager.SetInactiveButtonUI();
        }
    }

    public void ReadSpawnTurretEggnog(InputAction.CallbackContext context)
    {
        string turretType = "Eggnog";
        if (context.performed)
        {
            SpawnTurret(turretType);
            playerInput.SwitchCurrentActionMap("Player");
            playerManager.setInactiveTowersUI();
            playerManager.SetInactiveRemoveTurretUI();
            playerManager.SetInactiveButtonUI();
        }
    }

    public void ReadSpawnTurretSquirrel(InputAction.CallbackContext context)
    {
        string turretType = "Squirrel";
        if (context.performed)
        {
            SpawnTurret(turretType);
            playerInput.SwitchCurrentActionMap("Player");
            playerManager.setInactiveTowersUI();
            playerManager.SetInactiveRemoveTurretUI();
            playerManager.SetInactiveButtonUI();
        }
    }

    private void SpawnTurret(string turretType = "Penguin")
    {
        if (!highlight.gameObject.activeSelf) return;
        if (currentSlot == null) return;

        if (currentSlot.isOccupied)
        {
            Debug.Log("Slot já ocupado.");
            return;
        }

        towerRequestManager.RequestTowerBuy(turretType);

        if (!towerRequestManager.isTowerPriced) return;

        int towerType = 0;
        switch (turretType)
        {
            case "Penguin":
                towerType = 0;
                break;
            case "Eggnog":
                towerType = 1;
                break;
            case "Squirrel":
                towerType = 2;
                break;
        }

        GameObject turret = Instantiate(turrets[towerType], highlight.position, Quaternion.identity, currentSlot.transform);
        currentSlot.Occupy();
        towerRequestManager.isTowerPriced = false;
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

            currentSlot = hit.collider.GetComponent<TowerSlot>();
        }
        else
        {
            highlight.gameObject.SetActive(false);
            currentSlot = null;
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