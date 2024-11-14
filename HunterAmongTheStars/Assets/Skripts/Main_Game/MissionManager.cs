using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MissionManager : Singleton<MissionManager>
{
    private Transform currentPlanet;
    private Transform targetPlanet;
    public KeyCode moveButton = KeyCode.Space;

    [Header("Mini games")]
    public List<MiniGame> encounters;
    public MiniGame mission;
    public MissionPoint missionPoint;

    [Header("UI")]
    public GameObject missionUI;
    [SerializeField] GameObject MoveButton;
    [SerializeField] GameObject StartButton;
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI DescriptionText;
    void Awake()
    {
        missionUI.SetActive(false);
    }
    private void Update()
    {
        if (targetPlanet != null && targetPlanet != currentPlanet && Input.GetKeyDown(moveButton))
        {
            ShipMovement.Instance.StartMovingToPlanet(targetPlanet);
        }
    }
    public void DisplayMissionUI(MissionPoint obj)
    {
        missionPoint = obj;
        mission = missionPoint.miniGame;
        missionUI.SetActive(true);
        NameText.text = mission.Name;
        DescriptionText.text = mission.Description;

        if (targetPlanet == currentPlanet && targetPlanet != null)
        {
            MoveButton.SetActive(false);
            StartButton.SetActive(true);
        }
        else
        {
            MoveButton.SetActive(true);
            StartButton.SetActive(false);
        }
    }
    public void CloseMissionUI()
    {
        missionPoint.DeselectPlanet();
        mission = null;
        missionUI.SetActive(false);
        NameText.text = null;
        DescriptionText.text = null;
        targetPlanet = null;
    }
    public void ArriveAtPlanet()
    {
        missionUI.SetActive(true);
        MoveButton.SetActive(false);
        StartButton.SetActive(true);
        currentPlanet = targetPlanet;
    }
    public void StartMission()
    {
        if (mission != null)
        {
            SceneLoader.Instance.LoadScene(mission.Scene);
            missionUI.SetActive(false);
        }
    }
    public void MoveToPlanet()
    {
        if (targetPlanet != null && targetPlanet != currentPlanet)
            ShipMovement.Instance.StartMovingToPlanet(targetPlanet);
    }
    public void SelectPlanet(Transform planet)
    {
        // Assign the selected planet as the target
        targetPlanet = planet;
    }
    public void RandomEvent()
    {
        StartCoroutine(Encounter());
    }
    private IEnumerator Encounter()
    {
        Debug.Log("RandomEvent");
        float randomTime = Random.Range(1f, 1.5f);

        yield return new WaitForSeconds(randomTime);

        if (Random.Range(0, 100) < 20) // 35% chance of being an alien parasite
        {
            int E = Random.Range(0, encounters.Count);

            Debug.Log("RandomEvent");
            SceneLoader.Instance.LoadScene(encounters[E].Scene);
        }
    }
}
