using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : Singleton<MissionManager>
{
    public MiniGame mission;
    public MissionPoint missionPoint;
    public GameObject missionUI;
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI DescriptionText;
    void Awake()
    {
        missionUI.SetActive(false);
    }
    public void DisplayMissionUI(MissionPoint obj)
    {
        missionPoint = obj;
        mission = missionPoint.miniGame;
        missionUI.SetActive(true);
        NameText.text = mission.Name;
        DescriptionText.text = mission.Description;
    }
    public void CloseMissionUI()
    {
        missionPoint.DeselectPlanet();
        mission = null;
        missionUI.SetActive(false);
        NameText.text = null;
        DescriptionText.text = null;
    }
    public void StartMission()
    {
        SceneLoader.Instance.LoadScene(mission.Scene);
        missionUI.SetActive(false);
    }
}
