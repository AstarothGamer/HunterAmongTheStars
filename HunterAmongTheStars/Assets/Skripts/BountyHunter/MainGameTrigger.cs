using TMPro;
using UnityEngine;

public class MainGameTrigger : MonoBehaviour
{
    public Color highlightColor = Color.yellow;
    private Color originalColor;

    [SerializeField] private Renderer render;
    [SerializeField] private GameObject HintUI;

    void Start()
    {
        originalColor = render.material.color; // Store the original color
        HintUI.SetActive(false);
    }
    void OnMouseOver()
    {
        // Change the color of the mag
        render.material.color = highlightColor;
        HintUI.SetActive(true);

        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.PlaySound(SoundType.Button, 0.7f);
            SceneLoader.Instance.LoadScene("MainGame");
        }
    }
    void OnMouseExit()
    {
        render.material.color = originalColor;
        HintUI.SetActive(false);
    }
}
