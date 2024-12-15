using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class ArcadeTrigger : MonoBehaviour
{
    private Renderer render;
    public Color highlightColor = Color.yellow;
    private Color originalColor;

    [SerializeField] private GameObject HintUI;
    [SerializeField] private GameObject ArcadeUI;

    void Start()
    {
        render = GetComponent<Renderer>();
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
            SceneLoader.Instance.LoadScene("MemoryGame");
        }
    }
    void OnMouseExit()
    {
        render.material.color = originalColor;
        HintUI.SetActive(false);
    }
}
