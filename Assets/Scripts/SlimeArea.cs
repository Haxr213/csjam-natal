using UnityEngine;

public class SlimeArea : MonoBehaviour
{
    [SerializeField] private float slowMultiplier = 0.4f;
    [SerializeField] private float duration = 3f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyMoviment enemy))
        {
            enemy.ApplySlow(slowMultiplier);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyMoviment enemy))
        {
            enemy.RemoveSlow();
        }
    }
}
