using UnityEngine;

public class ShipAI : Damageable
{
    [SerializeField] protected Transform target;
    [SerializeField] protected float rotationalDamp = 0.5f;
    [SerializeField] protected float speed = 10f;

    public virtual void Update()
    {
        Turn();
        Move();
    }
    public virtual void Turn()
    {
        Vector3 pos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
    }
    void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
