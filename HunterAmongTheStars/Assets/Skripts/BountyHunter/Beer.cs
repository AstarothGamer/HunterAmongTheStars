using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class Beer : MonoBehaviour
{
    private Renderer mugRenderer;
    private DrunkEffect drunkEffect;
    public Color highlightColor = Color.yellow;
    private Color originalColor;
    private bool filled;

    [SerializeField] private GameObject beerUI;
    [SerializeField] private TextMeshProUGUI beerText;
    [SerializeField] private float duration = 15f;
    [SerializeField] private Volume volume;

    void Start()
    {
        mugRenderer = GetComponent<Renderer>();
        originalColor = mugRenderer.material.color; // Store the original color
        beerText.text = "drink the beer";
        beerUI.SetActive(false);
        filled = true;
        drunkEffect = volume.GetComponent<DrunkEffect>();
    }
    void OnMouseOver()
    {
        // Change the color of the mag
        mugRenderer.material.color = highlightColor;
        beerUI.SetActive(true);

        if (Input.GetMouseButtonDown(0))
        {
            if (filled)
            DrinkBeer();
            else
            FillBeer();
        }
    }
    void OnMouseExit()
    {
        mugRenderer.material.color = originalColor;
        beerUI.SetActive(false);
    }
    void DrinkBeer()
    {
        drunkEffect.TriggerDizzyness();
        filled = false;
        beerText.text = "10 credits to fill the beer";
        StartCoroutine(timer());
    }
    private IEnumerator timer()
    {
        yield return new WaitForSeconds(duration);
        drunkEffect.StopDizzyness();
    }
    void FillBeer()
    {
        // Spend money, change model
        filled = true;
        beerText.text = "drink the beer";
    }

}
