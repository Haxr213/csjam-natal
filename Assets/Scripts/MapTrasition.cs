using Unity.Cinemachine;
using UnityEngine;

public class MapTrasition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundry;
    [SerializeField] Direction direction;
    [SerializeField] float offset = 1f;
    CinemachineConfiner2D confiner;
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    private void Awake()
    {
        confiner = FindObjectsByType<CinemachineConfiner2D>(FindObjectsSortMode.None)[0];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //confiner.m_BoundingShape2D = mapBoundry;
            confiner.BoundingShape2D = mapBoundry;
            UpdatePlayerPosition(other.gameObject);
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPosition = player.transform.position;
        switch (direction)
        {
            case Direction.Up:
                newPosition.y += offset;
                break;
            case Direction.Down:
                newPosition.y -= offset;
                break;
            case Direction.Left:
                newPosition.x -= offset;
                break;
            case Direction.Right:
                newPosition.x += offset;
                break;
        }
        player.transform.position = newPosition;
    }
}
