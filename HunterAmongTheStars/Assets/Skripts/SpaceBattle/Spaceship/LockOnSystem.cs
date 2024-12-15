using UnityEngine;
using System.Collections.Generic;

public class LockOnSystem : MonoBehaviour
{
    public float lockOnRange = 50f;
    public float lockOnAngle = 30f;
    public LayerMask enemyLayer;
    public Transform lockOnTarget;
    [SerializeField] Camera cam;

    [Header("Mission Objective")]
    public Transform missionObjective; // location in the world
    public GameObject missionMarker; // UI marker
    private RectTransform missionMarkerRect; // RectTransform for the mission marker

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

        if (missionMarker != null)
        {
            missionMarkerRect = missionMarker.GetComponent<RectTransform>();
            missionMarker.SetActive(false);
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

        // Update mission marker
        UpdateMissionMarker();
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
    void UpdateMissionMarker()
    {
        if (missionMarker == null || missionObjective == null)
            return;

        // Convert world position to screen position
        Vector3 screenPosition = cam.WorldToScreenPoint(missionObjective.position);

        // Check if the objective is in front of the camera
        if (screenPosition.z > 0)
        {
            screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
            screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height);
            missionMarkerRect.position = screenPosition;
            missionMarker.SetActive(true);
        }
        else
        {
            Vector3 cameraToObjective = missionObjective.position - cam.transform.position;
            cameraToObjective.y = 0; // Ignore vertical component
            Vector3 forwardProjection = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;

            float angle = Vector3.SignedAngle(forwardProjection, cameraToObjective, Vector3.up);
            missionMarkerRect.rotation = Quaternion.Euler(0, 0, -angle);

            missionMarker.SetActive(true);
        }
    }
}