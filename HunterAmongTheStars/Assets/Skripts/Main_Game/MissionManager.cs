using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : Singleton<MissionManager>
{
    public MiniGame mission;
    public MissionPoint missionPoint;
    private Transform currentPlanet;
    private Transform targetPlanet;
    public KeyCode moveButton = KeyCode.Space;
    public GameObject missionUI;
    public GameObject MoveButton;
    public GameObject StartButton;
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
        if (targetPlanet != currentPlanet)
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
}
