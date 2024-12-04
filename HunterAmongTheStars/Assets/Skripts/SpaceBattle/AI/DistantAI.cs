using UnityEngine;

public class DistantAI : ShipAI
{
    public enum AIState { Chase, Retreat }
    public AIState currentState;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnpoint;
    [SerializeField] Transform projectileSpawnpoint2;

    [Header("Shooting")]
    [SerializeField] int projectilesPerShot = 1;
    [SerializeField] float attackRange = 15f;
    [SerializeField] float ShootCooldown = 4f;
    [SerializeField] float projectileSpread = 15f;
    [SerializeField] float projectileLifetime = 3;
    [SerializeField] float damage = 25f;
    [SerializeField] float projectileSpeed = 20f;
    private bool CanShoot = true;

    [Header("Sound Effects")]
    public bool LightShot = true;
    public bool HeavyShot = false;

    [Header("Visual Effects")]
    [SerializeField] ParticleSystem hit;

    private KillAlll killAll;
    protected override void Initialize()
    {
        base.Initialize();

        currentState = AIState.Chase;

        if (target == null)
        {
            target = PlayerManager.Instance.player.transform;
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }

        killAll = KillAlll.Instance;
        if (killAll != null)
         killAll.enemies++;
    }
    public override void Update()
    {
        switch (currentState)
        {
            case AIState.Chase:
                ChasePlayer();
                if (IsInAttackRange())
                    ChangeState(AIState.Retreat);
                break;

            case AIState.Retreat:
                Retreat();
                if (!IsInAttackRange())
                    ChangeState(AIState.Chase);
                break;
        }

        Turn();
    }

    void ChangeState(AIState newState)
    {
        currentState = newState;
    }

    void ChasePlayer()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    void AttackPlayer()
    {
        if (CanShoot)
        {
            Debug.Log("Shooting");
            ShootAtPlayer();
            CanShoot = false;
            Invoke("Cooldown", ShootCooldown);
        }
    }
    void Cooldown()
    {
        CanShoot = true;
    }
    void ShootAtPlayer()
    {
        if (LightShot)
            AudioManager.PlaySoundAtPoint(SoundType.SpaceshipLightShot, transform.position, 1f);
        if (HeavyShot)
            AudioManager.PlaySoundAtPoint(SoundType.SpaceshipHeavyShot, transform.position, 1f);

        for (int i = 0; i < projectilesPerShot; i++)
        {
            var go = Instantiate(projectilePrefab, projectileSpawnpoint.position, GetProjectileDirection(projectileSpread));
            var proj = go.GetComponent<EnemyBullet>();
            InitializeProjectile(proj);
        }

        if (projectileSpawnpoint2 != null)
        {
            for (int i = 0; i < projectilesPerShot; i++)
            {
                var go = Instantiate(projectilePrefab, projectileSpawnpoint2.position, GetProjectileDirection(projectileSpread));
                var proj = go.GetComponent<EnemyBullet>();
                InitializeProjectile(proj);
            }
        }

    }
    protected virtual void InitializeProjectile(EnemyBullet projectile)
    {
        projectile.Initialize(damage, projectileSpeed, projectileLifetime);
    }
    protected Quaternion GetProjectileDirection(float currentAccuracy)
    {
        float adjustedSpread = Random.Range(-currentAccuracy, currentAccuracy);
        return Quaternion.Euler(projectileSpawnpoint.rotation.eulerAngles + Vector3.forward * adjustedSpread);
    }

    void Retreat()
    {
        Vector3 directionAway = (transform.position - target.position).normalized;
        transform.position += directionAway * speed * Time.deltaTime;

        AttackPlayer();
    }

    // Utility functions for detecting player, taking cover, etc.
    bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, target.position) <= attackRange;
    }

    public override void Damage(float damage)
    {
        if (isDead || !isVulnerable || damage <= 0)
            return;

        currentHealth -= damage;

        if(hit != null)
        hit.Play();
        AudioManager.PlaySound(SoundType.Hit, 0.4f);

        if (currentHealth <= 0)
        {
            AudioManager.PlaySound(SoundType.Explosion, 0.3f);
            AudioManager.PlaySoundAtPoint(SoundType.Explosion, gameObject.transform.position, 1);
            if (killAll != null)
            killAll.ImDead();
            Die();
        }
    }
}
