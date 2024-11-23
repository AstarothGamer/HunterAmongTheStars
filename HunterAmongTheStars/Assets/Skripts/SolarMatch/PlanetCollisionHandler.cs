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
        // // Проверяем, если объект ниже границы
        // if (transform.position.y <= bottomBoundary.position.y)
        // {
        //     spawner.OnPlanetMissed(); // Уведомляем спавнер о пропущенной планете
        //     Destroy(gameObject); // Удаляем планету
        // }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Divider"))
        {
            spawner.OnPlanetMissed();
            Destroy(gameObject);
            // Debug.Log("Game Ended");
        }
    }
}
