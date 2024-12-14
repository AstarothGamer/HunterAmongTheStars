using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomPlanetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject restartMenuButton;
    [SerializeField] private GameObject startMenuButton;
    [SerializeField] private GameObject planetHandler;
    [SerializeField] private TMP_Text outcomeText;

    [SerializeField] private TMP_Text timerText;

    [SerializeField] private List<GameObject> planets; 
    [SerializeField] private List<Vector3> spawnPos;
    [SerializeField] private Transform bottomBoundary; 

    [SerializeField] private float spawnInterval = 0.8f; 
    [SerializeField] private float fallSpeed = 4f; 
    [SerializeField] private TMP_Text scoreText;

    private PlayerInputHandler player;

    private Queue<int> spawnQueue = new Queue<int>(); 
    private List<GameObject> spawnedPlanets = new List<GameObject>(); 

    public bool isGameActive = true;

    private int score;
    private float timer;

    private void Start()
    {
        AudioManager.PlayMusic(SoundType.BackgroundMusic, 0.2f);

        StartCoroutine(SpawnPlanets());
    }

    private void Update()
    {
        if(isGameActive)
        {
            timer += Time.deltaTime;
            timerText.text = "Timer: " + timer.ToString("0.00");
        }

        scoreText.text = "Score " + score;
        GameWon();
    }

    public IEnumerator SpawnPlanets()
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

        if(!isGameActive) 
        {
            yield return new WaitForSeconds(5);

            SceneManager.LoadScene("MemoryGame");
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
        outcomeText.text = "The planet touched the battier. You lost";
        Debug.Log("Game ended! Planet touched a lower barier!");
    }

    private void GameWon()
    {
        if(timer > 30)
        {
            isGameActive = false;

            outcomeText.text = "You survived for 30 seconds and won the game!";
        }
    }

}
