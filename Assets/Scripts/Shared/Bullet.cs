using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Enemy target;
    private float dmg;
    public float velocity = 90;
    private Vector3 direction; // Direção do movimento
    private Tower tower;

    public void SetBullet(Enemy target, float dmg, Tower tower)
    {
        this.target = target;
        this.dmg = dmg;
        this.tower = tower;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized; // Normaliza a direção para garantir que tenha magnitude 1
    }

    void Update()
    {
        if(target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, velocity * Time.deltaTime);
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= 0.1f)
            {
                tower.ApplyDamage(dmg); 
                Destroy(gameObject);
            }
            else if (distance > 0.1f && target.isDead)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
