using UnityEngine;
using System.Collections.Generic;

public class LockOnSystem : MonoBehaviour
{
    public float lockOnRange = 50f;
    public float lockOnAngle = 30f;
    public LayerMask enemyLayer;
    public Transform lockOnTarget;
    [SerializeField] Camera cam;

    [Header("Lock-On Indicator")]
    public GameObject lockOnIndicator; //  indicator for lock-on targets
    private RectTransform lockOnIndicatorRect; // RectTransform for the 2D indicator

    private List<ShipWeapon> weapons;

    void Start()
    {
        // Find all ShipWeapon components attached to the ship
        weapons = new List<ShipWeapon>(GetComponentsInChildren<ShipWeapon>());
        if (lockOnIndicator != null)
        {
            lockOnIndicatorRect = lockOnIndicator.GetComponent<RectTransform>();
            lockOnIndicator.SetActive(false);
        }
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
            if (lockOnTarget != null)
            {
                lockOnIndicator.SetActive(true);

                // Convert world position to screen position
                Vector3 screenPosition = cam.WorldToScreenPoint(lockOnTarget.position);

                // Check if the target is in front of the camera
                if (screenPosition.z > 0)
                {
                    // Update indicator position
                    lockOnIndicatorRect.position = screenPosition;
                }
                else
                {
                    // Hide indicator if the target is behind the camera
                    lockOnIndicator.SetActive(false);
                }
            }
            else
            {
                lockOnIndicator.SetActive(false);
            }
        }
    }

    Transform FindLockOnTarget()
    {
        Collider[] hits = Physics.OverlapSphere(cam.transform.position, lockOnRange, enemyLayer);

        Transform closestTarget = null;
        float closestDistance = lockOnRange;

        foreach (var hit in hits)
        {
            Vector3 directionToTarget = (hit.transform.position - cam.transform.position).normalized;
            float angleToTarget = Vector3.Angle(cam.transform.forward, directionToTarget);

            if (angleToTarget < lockOnAngle)
            {
                float distanceToTarget = Vector3.Distance(cam.transform.position, hit.transform.position);
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