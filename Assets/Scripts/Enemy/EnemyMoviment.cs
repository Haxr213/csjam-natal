using UnityEngine;

public class EnemyMoviment : MonoBehaviour
{
    public LevelManager levelManager;
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Animator anim;
    private Transform target; 
    private int pathIndex = 0;

    private void Start()
    {
        target = levelManager.path[pathIndex];
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) < 0.1f)
        {
            pathIndex++;
            if (pathIndex == levelManager.path.Length)
            {
                Debug.Log("Enemy reached the end of the path.");
                Destroy(gameObject);
                return;
            }
            target = levelManager.path[pathIndex];
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        int index = GetDirectionIndex(direction);
        anim.SetFloat("directionIndex", index);
    }

    private int GetDirectionIndex(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            // Horizontal
            if (dir.x > 0) return 3; // Right
            else return 1;           // Left
        }
        else
        {
            // Vertical
            if (dir.y > 0) return 0; // Up
            else return 2;           // Down
        }
    }
}
