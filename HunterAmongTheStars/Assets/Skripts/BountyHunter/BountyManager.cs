using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private GameObject outcomeUI;
    [SerializeField] private TMP_Text outcomeText;


    public bool isGameActive = false;


    private void Start()
    {
        if (Random.Range(0, 100) > 60) // Chance to spawn a bounty
        {
            AudioManager.PlayMusic(SoundType.BackgroundMusic3, 0.2f);
            isGameActive = true;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
            Cursor.lockState = CursorLockMode.None;
        }
    }


    public void CheckBounty(GameObject bounty)
    {
        if(isGameActive)
        {
            var identifier = bounty.GetComponent<BountyIdentifier>();
            if (identifier != null && identifier.bountyData == currentBounty)
            {
                isGameActive = false;
                bountyUI.SetActive(false);
                AudioManager.StopLoopSoundGradually(1);
                AudioManager.PlaySound(SoundType.Money);
                outcomeText.text = "You found your target and cought it.";
                StartCoroutine(timer());
                Debug.Log("You found the bounty!");
                AudioManager.PlayMusic(SoundType.BackgroundMusic4, 0.2f);
            }
            else
            {
                outcomeText.text = "It's the wrong target. Continue your searching";
                StartCoroutine(timerText());
                Debug.Log("This is the wrong person!");
            }
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

    private IEnumerator timer()
    {
        yield return new WaitForSeconds(5);
        outcomeUI.SetActive(false);
    }

    private IEnumerator timerText()
    {
        yield return new WaitForSeconds(3);
        outcomeText.text = "";
    }
}