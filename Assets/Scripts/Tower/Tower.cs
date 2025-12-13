using System.Collections;
using UnityEditor;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask targetLayerMask;

    public string towerName;
    private Transform target;
    private Animator animTower;
    [SerializeField]
    private Animator animShot;
    private float directionIndex;
    private bool canShoot = true;
    public Enemy currentTarget;
    public TowerData currentData;
    public Transform shootPosition;
    public Vector3 positionNode;

    public Bullet bullet;

    [SerializeField]
    private bool eggnog;
    [SerializeField]
    private SlimeSpawner slimeSpawner;

    [SerializeField]
    private AudioClip audioAttack;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animTower = GetComponentInChildren<Animator>();


        StartCoroutine(ShootTimer());
    }

    private void Update()
    {
        //Debug.Log( "Target:" + target);
        if(!IsTargetValid())
        {
            target = null;
            currentTarget = null;
        }

        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateGunTowardsTarget();
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, currentData.range, Vector2.zero, 0f, targetLayerMask);
        Debug.Log("Hits found: " + hits.Length);
        Debug.Log("TargetLayerMask: " + targetLayerMask.value);

        if(hits.Length > 0)
        {
            Transform nearest = hits[0].transform;
            float nearestDist = Vector2.Distance(transform.position, nearest.position);

            foreach (var h in hits)
            {
                float d = Vector2.Distance(transform.position, h.transform.position);
                if (d < nearestDist)
                {
                    nearest = h.transform;
                    nearestDist = d;
                }
            }

            target = nearest;
            currentTarget = target.GetComponent<Enemy>();
        }
    }

    private void RotateGunTowardsTarget()
    {
        if (target == null) return;

        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;

        float normalizedAngle = angle < 0 ? angle + 360f : angle;

        // 6 direções → cada uma é 60 graus
        if (eggnog)
            directionIndex = Mathf.FloorToInt(normalizedAngle / 90f);
        else
            directionIndex = Mathf.FloorToInt(normalizedAngle / 60f);

        animTower.SetFloat("directionIndex", directionIndex);
    }

    private bool IsTargetValid()
    {
        if (currentTarget == null) return false;
        if (currentTarget.isDead) return false;

        float distance = Vector2.Distance(transform.position, currentTarget.transform.position);
        return distance <= currentData.range;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, currentData.range);
    }

    public void SpawnerEggnog()
    {
        if (currentTarget != null && !currentTarget.isDead)
        {
            audioSource.PlayOneShot(audioAttack);
            ApplyDamage(currentData.dmg);

            if (eggnog) // se essa torre for a de gosma
            {
                slimeSpawner.SpawnSlimeNearTower(currentTarget.transform.position);
            }
        }
    }

    // Método que aplica dano
    public void ApplyDamage(float damage)
    {
        if (currentTarget != null && !currentTarget.isDead)
        {
            currentTarget.TakeDamage(damage);
        }
    }

    public void SetAttackSpeed(float newAttackSpeedMultiplier)
    {
        // Atualiza a velocidade da animação de ataque
        if (animTower != null)
        {
            animTower.SetFloat("AttackSpeed", newAttackSpeedMultiplier);
        }
    }

    //Tempo para atirar
    public IEnumerator ShootTimer()
    {
        while (true)
        {
            // Verifica se há um alvo
            if (IsTargetValid())
            {
                if (canShoot)
                {
                    canShoot = false;

                    // Rotaciona suavemente para olhar para o alvo
                    Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
                    direction.y = 0; // Ignora o componente Y

                    if (!eggnog)
                        animShot.SetTrigger("Attack");
                    
                    animTower.SetTrigger("Attack");

                    Debug.Log($"Atirando! Tempo de espera: {currentData.timeToShot} segundos.");
                    yield return new WaitForSeconds(0.2f); // Pequeno delay antes de atirar para sincronizar com a animação
                    Shoot();

                    // Aguarda o tempo definido em timeToShot antes de permitir um novo ataque
                    yield return new WaitForSeconds(currentData.timeToShot);

                    // Permite um novo ataque
                    canShoot = true;
                }
            }
            else
            {
                // Se não houver alvo, espera o próximo frame
                yield return null;
            }
        }
    }

    private void Shoot()
    {
        if (bullet != null && shootPosition != null && currentTarget != null)
        {
            // Calcula a direção do disparo (do monstro para o inimigo)
            Vector3 direction = (currentTarget.transform.position - shootPosition.position).normalized;

            // Instancia a bala
            var bulletGo = Instantiate(bullet, shootPosition.position, Quaternion.identity);

            // Define a direção do projétil
            bulletGo.SetBullet(currentTarget, currentData.dmg, this);
            bulletGo.SetDirection(direction);
            audioSource.PlayOneShot(audioAttack);

        }
        else
        {
            SpawnerEggnog();
        }
    }
}
