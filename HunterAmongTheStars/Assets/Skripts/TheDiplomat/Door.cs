using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private Renderer doorRenderer;
    [SerializeField] private GameObject doorUI;

    public DialogSystem dialog;

    public bool isDialogActive = true;
    private Color originalColor;
    private Color highlightColor = Color.white;

    void Start()
    {
        originalColor = doorRenderer.material.color;
    }
    void OnMouseOver()
    {
        // Change the color of the mag
        doorRenderer.material.color = highlightColor;
        if(!isDialogActive)
        {
            doorUI.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!isDialogActive)
            {
                doorUI.SetActive(false);
                SceneManager.LoadScene("Bar");
            }
        }
    }
    void OnMouseExit()
    {
        doorRenderer.material.color = originalColor;
        doorUI.SetActive(false);
    }
}
