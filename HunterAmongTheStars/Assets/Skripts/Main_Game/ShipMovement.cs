using Unity.Cinemachine;
using System.Collections;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private GameDataSO gameData;
    public Transform ship;
    private Transform targetPlanet;
    public float speed = 5f;
    public float rotationSpeed = 2f;
    public bool isMoving = false;
    public CinemachineCamera Cam;
    public float startDuration = 2.5f;

    private static ShipMovement _instance;

    #region Singleton
    public static ShipMovement Instance
    {
        get
        {
            // Check if the instance is already created
            if (_instance == null)
            {
                // Try to find an existing AudioManager in the scene
                _instance = FindAnyObjectByType<ShipMovement>();

                // If no AudioManager exists, create a new one
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("ShipMovement");
                    _instance = singletonObject.AddComponent<ShipMovement>();
                }

                // Make the AudioManager persist across scenes (optional)
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        // If the instance is already set, destroy this duplicate object
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;  // Assign this object as the instance
        }

        ship.position = gameData.playerPosition;
    }
    #endregion

    void Start()
    {
        AudioManager.PlayMusic(SoundType.SpaceMusic, 0.7f);
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

        AudioManager.PlayLoopSound(SoundType.Boost, 0.4f);
        MissionManager.Instance.RandomEvent();
    }
    public void StartMovingToPlanet(Transform target)
    {
        if (!isMoving)
        {
            targetPlanet = target;

            MissionManager.Instance.missionUI.SetActive(false);
            // Switch to the ship camera
            Cam.Priority = 30;

            StartCoroutine(Prepare());

            Debug.Log("Ship is moving towards: " + targetPlanet.name);
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
                if (Vector3.Distance(ship.position, targetPlanet.position) < 3f)
                {
                    Arrive();
                }
            }
        }

    }
    void Arrive()
    {
        if(gameData != null)
        {
            gameData.playerPosition = ship.position;
        }

        AudioManager.StopLoopSoundGradually(0.5f);
        MissionManager.Instance.ArriveAtPlanet();
        isMoving = false;
        Cam.Priority = 1;
    }
    public void SelectPlanet(Transform planet)
    {
        // Assign the selected planet as the target
        targetPlanet = planet;
    }
}