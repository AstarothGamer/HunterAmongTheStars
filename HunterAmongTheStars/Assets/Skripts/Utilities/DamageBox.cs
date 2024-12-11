using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageBox : MonoBehaviour
{
    [SerializeField] float damage;
    private bool can = true;
    private void OnTriggerEnter(Collider collision)
    {
        if (!can)
        return;

        var targetable = collision.GetComponentInParent<PlayerHP>();

        if (targetable == null)
        return;

        targetable.DamagePlayer(damage);
        can = false;
        Invoke("Timer", 1f);
    }
    private void Timer()
    {
        can = true;
    }
}
