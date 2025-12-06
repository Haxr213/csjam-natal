using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask targetLayerMask;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;

    private Transform target;

    private void Update()
    {
        //Debug.Log( "Target:" + target);
        if(target == null)
        {
            FindTarget();
            return;
        }

        RotateGunTowardsTarget();
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2) transform.position, 0f, targetLayerMask);
        Debug.Log("Hits found: " + hits.Length);
        Debug.Log("TargetLayerMask: " + targetLayerMask.value);

        if(hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void RotateGunTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = targetRotation; 

        //Debug.Log("Target rotation:" + targetRotation);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
