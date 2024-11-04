using Unity.Cinemachine;
using UnityEngine;

public class MissionPoint : MonoBehaviour
{
    private Renderer planetRenderer;
    public Color highlightColor = Color.yellow;
    private Color originalColor;
    public CinemachineCamera planetCam;
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
        MissionManager.Instance.DisplayMissionUI(this);
        ShipMovement.Instance.SelectPlanet(transform);
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
        MissionManager.Instance.CloseMissionUI();

    }
}