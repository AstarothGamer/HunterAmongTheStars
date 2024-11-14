using UnityEditorInternal.VersionControl;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public partial class Projectile : MonoBehaviour
{
    //these values are set by the weapon shooting them
    public float damage;
    public float speed;
    public bool shootThrough;
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
        var targetable = collision.GetComponent<Damageable>();

        if (targetable == null)
        {
            Impact(collision, true);
            return;
        }

        targetable.Damage(damage);
        Impact(collision, shootThrough);
    }

    //Inheriting classes can override this to have different
    //impact behaviors (such as bouncing on walls, or piercing through enemies)
    public void Impact(Collider collision, bool through)
    {
        if (impactEffect)
            Instantiate(impactEffect, transform.position, transform.rotation);

        if (through) 
        return;

        enabled = false;
        Destroy(gameObject);
    }
}