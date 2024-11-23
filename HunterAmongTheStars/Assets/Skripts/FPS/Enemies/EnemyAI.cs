using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : Damageable
{
    public enum AIState { Patrol, Chase, Attack, Retreat }
    public AIState currentState;

    public float sightRange = 35f;
    public float attackRange = 25f;
    public float alertRadius = 20f;
    public Transform[] patrolPoints;

    private NavMeshAgent agent;
    private int currentPatrolPoint = 0;
    private Transform player;

    private bool CanShoot;
    private bool hasAlerted = false;


    [Header("Weapon")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnpoint;

    [Header("General")]
    public float ShootCooldown = 2f;
    public float projectileSpeed = 40f;
    public int projectilesPerShot = 1;
    public float projectileLifetime = 3;
    public float projectileSpread = 0.5f;
    public float damage = 25f;

    [Header("Sound Effects")]
    public bool RifleSound = true;
    public bool PistolSound = false;
    public bool ShotgunSound = false;

    [Header("Vissual Effects")]
    [SerializeField] ParticleSystem blood;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerManager.Instance.player.transform;
        currentState = AIState.Patrol;
        CanShoot = true;
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Patrol:
                if (CanSeePlayer())
                {
                    Alert();
                    ChangeState(AIState.Chase);
                }
                Patrol();
                break;

            case AIState.Chase:
                ChasePlayer();
                if (IsInAttackRange() && CanSeePlayer())
                ChangeState(AIState.Attack);
                break;

            case AIState.Attack:
                AttackPlayer();
                if (!IsInAttackRange())
                ChangeState(AIState.Chase);
                break;

            case AIState.Retreat:
                Retreat();
                break;
        }
    }

    void ChangeState(AIState newState)
    {
        currentState = newState;
    }

    // Patrol state
    void Patrol()
    {
        if (patrolPoints.Length == 0)
        return;

        agent.SetDestination(patrolPoints[currentPatrolPoint].position);

        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 1f)
        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
    }
    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        LookAtPlayer();
    }
    void AttackPlayer()
    {
        LookAtPlayer();
        if (CanSeePlayer())
        {
            if (CanShoot)
            {
                Debug.Log("Shooting");
                ShootAtPlayer();
                CanShoot = false;
                Invoke("Cooldown", ShootCooldown);
            }
        }
    }
    void Cooldown()
    {
        CanShoot = true;
    }
    void ShootAtPlayer()
    {
        if (RifleSound)
            AudioManager.PlaySoundAtPoint(SoundType.RifleShot, projectileSpawnpoint.position, 1f);
        if (ShotgunSound)
            AudioManager.PlaySoundAtPoint(SoundType.ShotgunShot, projectileSpawnpoint.position, 1f);
        if (PistolSound)
            AudioManager.PlaySoundAtPoint(SoundType.PistolShot, projectileSpawnpoint.position, 1f);

        for (int i = 0; i < projectilesPerShot; i++)
        {
            var go = Instantiate(projectilePrefab, projectileSpawnpoint.position, GetProjectileDirection(projectileSpread));
            var proj = go.GetComponent<EnemyBullet>();
            InitializeProjectile(proj);
        }
    }
    protected virtual void InitializeProjectile(EnemyBullet projectile)
    {
        projectile.Initialize(damage, projectileSpeed, projectileLifetime);
    }
    protected Quaternion GetProjectileDirection(float currentAccuracy)
    {
        // Generate random offsets for pitch (x-axis) and yaw (y-axis)
        float spreadX = Random.Range(-currentAccuracy, currentAccuracy); // Vertical spread
        float spreadY = Random.Range(-currentAccuracy, currentAccuracy); // Horizontal spread

        // Apply the spread to the spawnpoint's forward direction
        return Quaternion.Euler(
            projectileSpawnpoint.rotation.eulerAngles.x + spreadX,
            projectileSpawnpoint.rotation.eulerAngles.y + spreadY,
            projectileSpawnpoint.rotation.eulerAngles.z
        );
    }

    void Retreat()
    {
        Vector3 retreatPosition = transform.position + (transform.position - player.position).normalized * 10f;
        agent.SetDestination(retreatPosition);
    }

    // Utility functions for detecting player, taking cover, etc.
    bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, sightRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true; 
            }
        }
        return false; 
    }
    private void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Adjust speed as needed
        }
    }
    public override void Damage(float damage)
    {
        if (isDead || !isVulnerable || damage <= 0)
            return;

        Alert();
        currentHealth -= damage;
        Debug.Log("Received " + damage + " damage");

        if (blood != null)
        blood.Play();

        AudioManager.PlaySoundAtPoint(SoundType.Damage, transform.position, 1f);
        AudioManager.PlaySound(SoundType.Hit, 0.4f);

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth < 55)
        {
            Debug.Log("Retreat!!!");
            ChangeState(AIState.Retreat);  // Flee if health is low
            Invoke("ComeBack", 15f);
        }
    }
    void ComeBack()
    {
        ChangeState(AIState.Chase);
    }
    void Alert()
    {
        if (hasAlerted) return;

        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, alertRadius, LayerMask.GetMask("Enemy"));
        foreach (Collider enemy in nearbyEnemies)
        {
            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null && ai != this) // Avoid notifying itself
            {
                ai.ChangeState(AIState.Chase);
            }
        }
        hasAlerted = true; // Prevent re-alerting in the same event
    }
}