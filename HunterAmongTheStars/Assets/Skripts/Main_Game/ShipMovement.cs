using Unity.Cinemachine;
using System.Collections;
using UnityEngine;

public class ShipMovement : Singleton<ShipMovement>
{
    public Transform ship;
    private Transform targetPlanet;
    public float speed = 5f;
    public float rotationSpeed = 2f;
    private bool isMoving = false;
    public CinemachineCamera Cam;
    public float startDuration = 2.5f;

    void Start()
    {
        Cam.Priority = 1;
    }
    void Update()
    {
         MoveShipToPlanet();
    }
    private IEnumerator Prepare()
    {
        yield return new WaitForSeconds(startDuration);
        
        if (targetPlanet != null)
        isMoving = true;
    }
    public void StartMovingToPlanet(Transform target)
    {
        if (!isMoving)
        {
            targetPlanet = target;

            if (targetPlanet != null)
            {
                MissionManager.Instance.missionUI.SetActive(false);
                // Switch to the ship camera
                Cam.Priority = 30;

                StartCoroutine(Prepare());

                Debug.Log("Ship is moving towards: " + targetPlanet.name);
            }
        }
    }

    void MoveShipToPlanet()
    {
        if (targetPlanet != null)
        {
            // Rotate the ship to face the planet
            Vector3 directionToPlanet = targetPlanet.position - ship.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlanet);
            ship.rotation = Quaternion.Slerp(ship.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (isMoving)
            {
                // Move the ship towards the planet
                ship.position = Vector3.MoveTowards(ship.position, targetPlanet.position, speed * Time.deltaTime);

                // Check if the ship has reached the planet
                if (Vector3.Distance(ship.position, targetPlanet.position) < 0.8f)
                {
                    Arrive();
                }
            }
        }

    }
    void Arrive()
    {
        MissionManager.Instance.ArriveAtPlanet();
        isMoving = false;
        Cam.Priority = 1;
    }
}