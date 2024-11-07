using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Damageable
{
    [SerializeField] Transform target;
    [SerializeField] float rotationalDamp = 0.5f;
    [SerializeField] float speed = 10f;

    // Update is called once per frame
    void Update()
    {
        Turn();
        Move();
    }
    void Turn()
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
