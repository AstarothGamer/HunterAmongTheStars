using UnityEngine;

public class Doors : MonoBehaviour
{
    public bool playerInRange;
    public bool CanInteract = false;
    public bool wave;
    public float Range;
    public GameObject UI;
    public Animator anim;
    public GameObject nextRoomWave;

    // Update is called once per frame
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
                if (wave)
                {
                    nextRoomWave.SetActive(true);
                }
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
