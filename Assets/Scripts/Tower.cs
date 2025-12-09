using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Tower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask targetLayerMask;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;

    public string towerName;
    private Transform target;
    private Animator anim;
    private float directionIndex;
    private bool canShoot = true;
    public Enemy currentTarget;
    public TowerData currentData;
    public Transform shootPosition;
    public Vector3 positionNode;

    public Bullet bullet;

    private bool hasAppliedDamage = false; // Variável de controle para evitar múltiplos danos
    private bool isAttacking = false; // Controla se está atacando
    [SerializeField]
    private bool isTwoAngles;

    [SerializeField]
    private AudioClip audioAttack;
    private AudioSource audioSource;

    private Coroutine shootTimerCoroutine;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();


        shootTimerCoroutine = StartCoroutine(ShootTimer());
    }

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
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, targetLayerMask);
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
        if (isTwoAngles)
            directionIndex = Mathf.FloorToInt(normalizedAngle / 90f);
        else
            directionIndex = Mathf.FloorToInt(normalizedAngle / 60f);

        anim.SetFloat("directionIndex", directionIndex);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    // Método chamado pela animação quando termina
    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
        hasAppliedDamage = false;
    }

    // Método chamado pela animação para aplicar o dano
    public void OnAttackAnimationEvent()
    {
        if (currentTarget != null && !currentTarget.isDead && !hasAppliedDamage)
        {
            audioSource.PlayOneShot(audioAttack);
            ApplyDamage(currentData.dmg);
            hasAppliedDamage = true; // Impede múltiplos danos
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
        if (anim != null)
        {
            anim.SetFloat("AttackSpeed", newAttackSpeedMultiplier);
        }
    }

    //Tempo para atirar
    private IEnumerator ShootTimer()
    {
        while (true)
        {
            // Verifica se há um alvo
            if (currentTarget != null)
            {
                if (canShoot)
                {
                    canShoot = false;

                    // Rotaciona suavemente para olhar para o alvo
                    Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
                    direction.y = 0; // Ignora o componente Y

                    anim.SetTrigger("Attack");

                    Debug.Log($"Atirando! Tempo de espera: {currentData.timeToShot} segundos.");
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
            Debug.LogError("Bullet, shootPosition ou currentTarget não atribuídos.");
        }
    }
}
