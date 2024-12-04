using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageBox : MonoBehaviour
{
    [SerializeField] float damage;
    private void OnTriggerEnter(Collider collision)
    {
        var targetable = collision.GetComponentInParent<PlayerHP>();

        if (targetable == null)
        return;

        targetable.DamagePlayer(damage);
    }
}
