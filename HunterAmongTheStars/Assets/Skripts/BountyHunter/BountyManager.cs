using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BountyManager : MonoBehaviour
{
    [SerializeField] private List<Bounty> bounties; // List of potential bounties
    public Bounty currentBounty;

    [Header("UI")]
    [SerializeField] private GameObject bountyUI; // The UI panel for displaying bounty details
    [SerializeField] private TextMeshProUGUI bountyName;
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private TextMeshProUGUI reward;

    [Header("Spawner")]
    [SerializeField] private List<GameObject> prefabs; // Non-bounty prefabs
    [SerializeField] private List<Transform> spawnPoints;

    private void Start()
    {
        SetRandomBounty();//for test

        if (Random.Range(0, 100) > 70) // Chance to spawn a bounty
        {
            SpawnBounty();
        }

        FillTheRoom(); // Spawn other NPCs
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            bountyUI.SetActive(!bountyUI.activeSelf);
        }
    }

    private void FillTheRoom()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            int R = Random.Range(0, prefabs.Count);
            Instantiate(prefabs[R], spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void CheckBounty(GameObject bounty)
    {
        if (bounty == currentBounty.prefab)
        {
            Debug.Log("You found the bounty!");
            SetRandomBounty();
        }
        else
        {
            Debug.Log("This is the wrong person!");
        }
    }

    private void SpawnBounty()
    {
        if (spawnPoints.Count > 0)
        {
            int R = Random.Range(0, spawnPoints.Count);
            Instantiate(currentBounty.prefab, spawnPoints[R].position, spawnPoints[R].rotation);
            spawnPoints.RemoveAt(R); // Avoid duplicate spawns at the same point
        }
        else
        {
            Debug.LogWarning("No spawn points available for bounty!");
        }
    }

    private void UpdateBountyUI()
    {
        if (currentBounty != null)
        {
            bountyName.text = currentBounty.name;
            image.sprite = currentBounty.bountyPhoto;
            reward.text = currentBounty.bountyReward.ToString();
        }
        else
        {
            Debug.LogWarning("No current bounty to update UI!");
        }
    }

    private void SetRandomBounty()
    {
        if (bounties.Count > 0)
        {
            currentBounty = bounties[Random.Range(0, bounties.Count)];
        }
        else
        {
            Debug.LogWarning("No bounties");
        }

        UpdateBountyUI();
    }
}