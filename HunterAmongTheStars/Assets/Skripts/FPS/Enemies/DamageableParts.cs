using UnityEngine;

public class DamageableParts : Damageable
{
    public float DamageModidfier = 1f;
    public Damageable Owner;

    public override void Damage(float damage)
    {
        float modifier = damage * DamageModidfier;
        Owner.Damage(modifier);
        Debug.Log("damage");
    }
}