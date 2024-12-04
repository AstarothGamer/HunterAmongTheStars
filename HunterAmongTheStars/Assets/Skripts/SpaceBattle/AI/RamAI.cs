using System.Collections;
using UnityEngine;

public class RamAI : ShipAI
{
    public enum AIState { Chase, Attack, Cooldown }
    public AIState currentState;

    [Header("Attacking")]
    [SerializeField] float attackRange = 15f;
    [SerializeField] float cooldown = 4f;
    [SerializeField] float ramDuration = 4f;
    [SerializeField] float ramSpeed = 20f;
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private float angle = 30f; // Angle for attack
    [SerializeField] GameObject damageBox; // Angle for attack
    private bool isRamming = false;


    [Header("Sound Effects")]
    public bool Ram = true;

    [Header("Visual Effects")]
    [SerializeField] ParticleSystem hit;
    [SerializeField] GameObject warning;

    private bool canAttack = true;
    private KillAlll killAll;

    protected override void Initialize()
    {
        base.Initialize();
        currentState = AIState.Chase;
        damageBox.SetActive(false);

        if (target == null)
        {
            target = PlayerManager.Instance.player.transform;
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }

        if (warning != null)
        warning.SetActive(false);

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
                if (IsInAttackRange() && IsPlayerInFront() && canAttack)
                    ChangeState(AIState.Attack);
                break;

            case AIState.Attack:
                StartCoroutine(RamPlayer());
                break;

            case AIState.Cooldown:
                // Wait for cooldown to finish
                break;
        }
    }

    void ChangeState(AIState newState)
    {
        currentState = newState;
    }

    void ChasePlayer()
    {
        if (!IsInAttackRange())
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        Turn();
    }

    IEnumerator RamPlayer()
    {
        if (isRamming) yield break;
        isRamming = true; 

        AudioManager.PlaySound(SoundType.Button2, 0.6f);
        canAttack = false;

        // Activate warning
        if (warning != null)
            warning.SetActive(true);

        // Wait for attack delay
        yield return new WaitForSeconds(attackDelay);

        // Deactivate warning
        if (warning != null)
            warning.SetActive(false);

        float elapsedTime = 0f;

        while (elapsedTime < ramDuration)
        {
            transform.position += transform.forward * ramSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            damageBox.SetActive(true);

            if (Vector3.Distance(transform.position, target.position) < 1f)
            {
                break;
            }

            yield return null;
        }

        damageBox.SetActive(false);

        // Transition to cooldown state
        ChangeState(AIState.Cooldown);
        StartCoroutine(CooldownRoutine());

        isRamming = false;
    }

    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
        ChangeState(AIState.Chase);
    }

    bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, target.position) <= attackRange;
    }

    bool IsPlayerInFront()
    {
        Vector3 directionToPlayer = (target.position - transform.position).normalized;
        float Angle = Vector3.Angle(transform.forward, directionToPlayer);
        return Angle <= angle;
    }

    public override void Damage(float damage)
    {
        if (isDead || !isVulnerable || damage <= 0)
            return;

        currentHealth -= damage;

        if (hit != null)
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