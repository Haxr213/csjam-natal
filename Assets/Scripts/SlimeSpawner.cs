using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private float spawnRange = 1.5f;
    [SerializeField] private float minDistanceBetweenSlimes = 0.8f;

    private List<Vector2> spawnedSlimesPositions = new List<Vector2>();

    public void SpawnSlimeNearTower(Vector3 enemyPosition)
    {
        Vector2 bestPoint = Vector2.zero;
        float bestDistance = Mathf.Infinity;
        bool found = false;

        var path = levelManager.path;

        for (int i = 0; i < path.Length - 1; i++)
        {
            Vector2 a = path[i].position;
            Vector2 b = path[i + 1].position;

            Vector2 projected = ProjectPointOnSegment(enemyPosition, a, b);
            float dist = Vector2.Distance(enemyPosition, projected);

            if (dist < bestDistance && dist <= spawnRange)
            {
                if (!IsTooCloseToOtherSlimes(projected))
                {
                    bestDistance = dist;
                    bestPoint = projected;
                    found = true;
                }
            }
        }

        if (found)
        {
            Instantiate(slimePrefab, bestPoint, Quaternion.identity);
            spawnedSlimesPositions.Add(bestPoint);
            Debug.Log("Slime spawnado no segmento do path");
        }
    }

    private bool IsTooCloseToOtherSlimes(Vector2 position)
    {
        foreach (var p in spawnedSlimesPositions)
        {
            if (Vector2.Distance(p, position) < minDistanceBetweenSlimes)
                return true;
        }
        return false;
    }

    private Vector2 ProjectPointOnSegment(Vector2 point, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float t = Vector2.Dot(point - a, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t);
        return a + ab * t;
    }
}
