using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyBullet : MonoBehaviour
{
    //these values are set by the weapon shooting them
    public float damage;
    public float speed;
    [SerializeField] GameObject impactEffect;

    public virtual void Initialize(float damage, float speed, float duration)
    {
        this.damage = damage;
        this.speed = speed;
        Destroy(gameObject, duration);
    }

    public void Update()
    {
        MoveUpdate();
    }

    //Inheriting classes can override this to move in a different way
    protected void MoveUpdate()
    {
        float moveby = speed * Time.deltaTime;
        transform.position += transform.forward * moveby;
    }

    private void OnTriggerEnter(Collider collision)
    {
        var targetable = collision.GetComponent<PlayerHP>();

        if (targetable == null)
        {
            Impact(collision, true);
            return;
        }

        targetable.DamagePlayer(damage);
        Impact(collision, false);
    }

    //Inheriting classes can override this to have different
    //impact behaviors (such as bouncing on walls, or piercing through enemies)
    public void Impact(Collider collision, bool hit)
    {
        if (impactEffect)
            Instantiate(impactEffect, transform.position, transform.rotation);

        if (!hit)
            return;

        enabled = false;
        Destroy(gameObject);
    }
}
