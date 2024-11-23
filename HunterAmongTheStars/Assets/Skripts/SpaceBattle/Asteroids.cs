using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    public GameObject asteroidPrefab;   // The asteroid prefab to spawn
    public int numberOfAsteroids = 100; // Number of asteroids to spawn
    public Vector3 areaSize = new Vector3(100, 100, 100); // Area size for random placement
    public float minDistanceBetweenAsteroids = 5f; // Minimum distance between asteroids
    public float minSize = 1f;
    public float maxSize = 5f;
    public List<GameObject> asteroids;

    private void Start()
    {
        GenerateAsteroidField();
    }

    void GenerateAsteroidField()
    {
        // List to hold all spawned asteroid positions to check for overlap
        Vector3[] asteroidPositions = new Vector3[numberOfAsteroids];

        for (int i = 0; i < numberOfAsteroids; i++)
        {
            Vector3 position = GetRandomPosition();

            // Ensure no overlap by checking distance from other asteroids
            bool tooClose = false;
            for (int j = 0; j < i; j++)
            {
                if (Vector3.Distance(position, asteroidPositions[j]) < minDistanceBetweenAsteroids)
                {
                    tooClose = true;
                    break;
                }
            }

            // If it's too close to any existing asteroid, retry the position
            if (tooClose)
            {
                i--; // Decrease index to retry this asteroid
                continue;
            }

            // Place the asteroid at the random position
            GameObject asteroid = Instantiate(asteroidPrefab, position, Random.rotation);
            // Randomize size within the defined range
            float randomSize = Random.Range(minSize, maxSize);
            // Set the randomized size
            asteroid.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
            asteroidPositions[i] = position;
        }
    }

    Vector3 GetRandomPosition()
    {
        // Generate a random position within a specified bounding box
        float x = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        float y = Random.Range(-areaSize.y / 2, areaSize.y / 2);
        float z = Random.Range(-areaSize.z / 2, areaSize.z / 2);

        return new Vector3(x, y, z);
    }
}
