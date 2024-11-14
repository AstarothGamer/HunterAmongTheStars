using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Damageable : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100f;
    public bool isDead;
    public bool isVulnerable = true;

    [Header("Visual Effects")]
    public GameObject Remains;

    protected void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        currentHealth = maxHealth;
    }

    //deals damage to this unit and checks if it's dead
    public virtual void Damage(float damage)
    {
        if (isDead || !isVulnerable || damage <= 0)
            return;

        Debug.Log("damage");
        currentHealth -= damage;
        //AudioManager.PlaySoundAtPoint(SoundType.Damage, gameObject.transform.position, 0.8f);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            //play SFX
        }
    }

    public void Heal(float amount)
    {
        if (isDead || amount <= 0)
            return;

        if (currentHealth + amount > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += amount;

        //play SFX
    }

    public virtual void Die()
    {
        isDead = true;
        if (Remains != null)
        Instantiate(Remains, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
