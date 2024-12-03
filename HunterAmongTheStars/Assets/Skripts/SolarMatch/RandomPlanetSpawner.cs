using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomPlanetSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> planets; 
    [SerializeField] private List<Vector3> spawnPos;
    [SerializeField] private Transform bottomBoundary; 
    [SerializeField] private float spawnInterval = 0.8f; 
    [SerializeField] private float fallSpeed = 4f; 
    [SerializeField] private TMP_Text scoreText;

    private Queue<int> spawnQueue = new Queue<int>(); 
    private List<GameObject> spawnedPlanets = new List<GameObject>(); 

    private bool isGameActive = true;

    private int score;

    private void Start()
    {
        StartCoroutine(SpawnPlanets());
    }

    private void Update()
    {
        scoreText.text = "Score " + score;
    }

    private IEnumerator SpawnPlanets()
    {
        while (isGameActive)
        {
            int randomIndex = Random.Range(0, planets.Count);
            spawnQueue.Enqueue(randomIndex); 
            
            GameObject planet = Instantiate(planets[randomIndex], spawnPos[Random.Range(0, spawnPos.Count)], Quaternion.identity);

            spawnedPlanets.Add(planet);

            Rigidbody rb = planet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.down * fallSpeed;
            }

            PlanetCollisionHandler collisionHandler = planet.AddComponent<PlanetCollisionHandler>();
            collisionHandler.Initialize(this, bottomBoundary);


            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public bool CheckPlanet(int planetIndex)
    {
        if (spawnQueue.Count > 0 && spawnQueue.Peek() == planetIndex)
        {
            spawnQueue.Dequeue(); 

            if (spawnedPlanets.Count > 0)
            {
                GameObject planetToDestroy = spawnedPlanets[0];
                spawnedPlanets.RemoveAt(0);
                Destroy(planetToDestroy);
            }

            score++;

            return true; 
        }

        return false; 
    }

     public void OnPlanetMissed()
    {
        isGameActive = false; 
        Debug.Log("Game ended! Planet touched a lower barier!");
    }
}
