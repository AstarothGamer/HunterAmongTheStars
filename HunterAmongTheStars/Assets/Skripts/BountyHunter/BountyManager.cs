using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BountyManager : MonoBehaviour
{
    [SerializeField] private List<Bounty> bounties; // List of potential bounties
    public Bounty currentBounty;

    [Header("UI")]
    [SerializeField] private GameObject bountyUI; // The UI panel for displaying bounty details
    [SerializeField] private TextMeshProUGUI bountyName;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI reward;

    [Header("Spawner")]
    [SerializeField] private List<GameObject> prefabs; // Non-bounty prefabs
    [SerializeField] private List<Transform> spawnPoints;

    private bool isGameActive = false;
    public BountyInteraction npcInteraction;

    private void Start()
    {
        if (Random.Range(0, 100) > 60) // Chance to spawn a bounty
        {
            AudioManager.PlayMusic(SoundType.BackgroundMusic3, 0.2f);
            isGameActive = true;
            npcInteraction.isGameActive = true;
            SetRandomBounty();
        }
        else
        {
            AudioManager.PlayMusic(SoundType.BackgroundMusic4, 0.2f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.B) && isGameActive)
        {
            bountyUI.SetActive(!bountyUI.activeSelf);
        }
    }


    public void CheckBounty(GameObject bounty)
    {
        var identifier = bounty.GetComponent<BountyIdentifier>();
        if (identifier != null && identifier.bountyData == currentBounty)
        {
            bountyUI.SetActive(false);
            AudioManager.StopLoopSoundGradually(1);
            npcInteraction.isGameActive = false;
            AudioManager.PlaySound(SoundType.Money);
            Debug.Log("You found the bounty!");
            AudioManager.PlayMusic(SoundType.BackgroundMusic4, 0.2f);
        }
        else
        {
            Debug.Log("This is the wrong person!");
        }
    }


    private void UpdateBountyUI()
    {
        if (currentBounty != null)
        {
            bountyName.text = currentBounty.name;
            image.sprite = currentBounty.bountyPhoto;
            reward.text = currentBounty.bountyReward.ToString();
            bountyUI.SetActive(true);
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