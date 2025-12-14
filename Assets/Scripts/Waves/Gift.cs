using UnityEngine;

public enum GiftQuality
{
    Good,
    Bad
}

public class Gift : MonoBehaviour
{
    [Header("Gift Settings")]
    public GiftQuality quality;
    public float moveSpeed = 2f;

    private LevelManager levelManager;
    private int pathIndex;
    private bool isPaused;

    public System.Action<Gift> OnGiftReachedEnd;

    public void Init(LevelManager manager, GiftQuality giftQuality)
    {
        levelManager = manager;
        quality = giftQuality;
        pathIndex = 0;
    }

    private void Update()
    {
        if (isPaused || levelManager == null) return;

        MoveAlongPath();
    }

    private void MoveAlongPath()
    {
        Transform target = levelManager.path[pathIndex];
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            pathIndex++;

            if (pathIndex >= levelManager.path.Length)
            {
                OnGiftReachedEnd?.Invoke(this);
                Destroy(gameObject);
            }
        }
    }

    public void PauseMovement()
    {
        isPaused = true;
    }

    public void ResumeMovement()
    {
        isPaused = false;
    }
}