using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private Renderer npcRenderer;
    [SerializeField] private GameObject npcUI;
    [SerializeField] private GameObject npcText;
    public DialogSystem dialog;

    private bool isDialogActive = false;

    void OnMouseOver()
    {
        // Change the color of the mag
        // npcRenderer.material.color = highlightColor;
        if(!isDialogActive)
        {
            npcUI.SetActive(true);
            npcText.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!isDialogActive)
            {
                dialog.SetRandomQuestion();
                dialog.isGameActive = true;
                isDialogActive = true;
                npcUI.SetActive(false);
                npcText.SetActive(false);
            }
        }
    }
    void OnMouseExit()
    {
        // mugRenderer.material.color = originalColor;
        npcUI.SetActive(false);
        npcText.SetActive(false);
    }

}
