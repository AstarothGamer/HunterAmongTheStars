using UnityEngine;

public class Doors : MonoBehaviour // I was to lasy to make a good cript
{
    bool CanInteract = false;
    [SerializeField] GameObject UI;
    [SerializeField] Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Show();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            DontShow();
        }
    }
    void Update()
    {

        if (CanInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.SetBool("Open", true);
                UI.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
    private void DontShow()
    {
        UI.SetActive(false);
        CanInteract = false;
    }
    private void Show()
    {
        UI.SetActive(true);
        CanInteract = true;
    }
}
