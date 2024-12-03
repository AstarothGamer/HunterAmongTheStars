using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageBox : MonoBehaviour
{
    [SerializeField] float damage;
    private void OnTriggerEnter(Collider collision)
    {
        var targetable = collision.GetComponent<Damageable>();

        if (targetable == null)
        return;

        targetable.Damage(damage);
    }
}
