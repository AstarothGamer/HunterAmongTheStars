using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class Beer : MonoBehaviour
{
    [SerializeField] Renderer mugRenderer;
    private DrunkEffect drunkEffect;
    public Color highlightColor = Color.yellow;
    private Color originalColor;
    private bool filled;

    [SerializeField] private GameObject beerUI;
    [SerializeField] private GameObject juice;
    [SerializeField] private GameObject foam;
    [SerializeField] private TextMeshProUGUI beerText;
    [SerializeField] private float duration = 15f;
    [SerializeField] private Volume volume;

    void Start()
    {
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
        AudioManager.PlaySound(SoundType.Item, 0.7f);
        juice.SetActive(false);
        foam.SetActive(false);
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
        AudioManager.PlaySound(SoundType.Money, 0.7f);
        juice.SetActive(true);
        foam.SetActive(true);
        // Spend money, change model
        filled = true;
        beerText.text = "drink the beer";
    }

}
