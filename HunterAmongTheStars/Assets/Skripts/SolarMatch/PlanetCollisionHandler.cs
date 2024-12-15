using UnityEngine;

public class PlanetCollisionHandler : MonoBehaviour
{
    private RandomPlanetSpawner spawner;
    private Transform bottomBoundary;

    public void Initialize(RandomPlanetSpawner spawner, Transform bottomBoundary)
    {
        this.spawner = spawner;
        this.bottomBoundary = bottomBoundary;
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Divider"))
        {
            spawner.OnPlanetMissed();
            Destroy(gameObject);
            Debug.Log("Game Ended");
        }
    }
}
