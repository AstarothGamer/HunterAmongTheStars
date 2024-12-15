using Microsoft.Win32.SafeHandles;
using Unity.Cinemachine;
using UnityEngine;

public class MissionPoint : MonoBehaviour
{
    private Renderer planetRenderer;
    public Color highlightColor = Color.yellow;
    private Color originalColor;
    [SerializeField] CinemachineCamera planetCam;
    [SerializeField] CinemachineCamera MainCam;
    public bool InFocus = false;

    [Header("MiniGame")]
    public MiniGame miniGame;

    void Start()
    {
        // Get the planet's renderer to modify its color on hover
        planetRenderer = GetComponent<Renderer>();
        originalColor = planetRenderer.material.color; // Store the original color
    }
    void OnMouseOver()
    {
        if (ShipMovement.Instance.isMoving)
            return;

        // Change the color of the planet
        if (!InFocus)
        planetRenderer.material.color = highlightColor;

        if (Input.GetMouseButtonDown(0))
        {
            SelectPlanet();
            InFocus = true;
        }
    }
    void OnMouseExit()
    {
        if (!InFocus)
        planetRenderer.material.color = originalColor;
    }

    // Function to handle what happens when the planet is selected
    void SelectPlanet()
    {
        Debug.Log("Planet Selected: " + gameObject.name);
        planetRenderer.material.color = originalColor;
        planetCam.Priority = 20;

        AudioManager.PlaySound(SoundType.Button, 0.7f);
        MissionManager.Instance.SelectPlanet(transform);
        ShipMovement.Instance.SelectPlanet(transform);
        MissionManager.Instance.DisplayMissionUI(this);
        AssignMission();
    }
    void AssignMission()
    {
        Debug.Log("Assigning a random mission to the planet...");
        // Random mission
    }
    public void DeselectPlanet()
    {
        if (!InFocus)
        return;

        InFocus = false;
        planetCam.Priority = 1;
        if (MainCam)
        {
            MainCam.GetComponent<CinemachinePanTilt>().PanAxis.Value = 0;
            MainCam.GetComponent<CinemachinePanTilt>().TiltAxis.Value = 45;
        }
        MissionManager.Instance.CloseMissionUI();
    }
}