using UnityEngine;
using System.Collections.Generic;

public class LockOnSystem : MonoBehaviour
{
    public float lockOnRange = 50f;
    public float lockOnAngle = 30f;
    public LayerMask enemyLayer;
    public Transform lockOnTarget;

    [Header("Lock-On Indicators")]
    public GameObject lockOnIndicator; // Optional: indicator for lock-on targets

    private List<ShipWeapon> weapons;

    void Start()
    {
        // Find all ShipWeapon components attached to the ship
        weapons = new List<ShipWeapon>(GetComponentsInChildren<ShipWeapon>());
    }

    void Update()
    {
        // Check for targets in front of the player
        lockOnTarget = FindLockOnTarget();

        // Update weapons with the lock-on target
        foreach (var weapon in weapons)
        {
            weapon.SetTarget(lockOnTarget);
        }

        // Update lock-on indicator
        if (lockOnIndicator != null)
        {
            lockOnIndicator.SetActive(lockOnTarget != null);
            if (lockOnTarget != null)
                lockOnIndicator.transform.position = lockOnTarget.position;
        }
    }

    Transform FindLockOnTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, lockOnRange, enemyLayer);

        Transform closestTarget = null;
        float closestDistance = lockOnRange;

        foreach (var hit in hits)
        {
            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget < lockOnAngle)
            {
                float distanceToTarget = Vector3.Distance(transform.position, hit.transform.position);
                if (distanceToTarget < closestDistance)
                {
                    closestTarget = hit.transform;
                    closestDistance = distanceToTarget;
                }
            }
        }

        return closestTarget;
    }
}