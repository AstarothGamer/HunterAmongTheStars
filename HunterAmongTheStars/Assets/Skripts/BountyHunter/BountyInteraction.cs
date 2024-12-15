using UnityEngine;

public class BountyInteraction : MonoBehaviour
{
    [SerializeField] private GameObject bountyInteraction;
    public BountyManager bounty;
    public bool isGameActive = false;

    void OnMouseOver()
    {
        if(isGameActive)
        {
            bountyInteraction.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                bounty.CheckBounty(gameObject);
            }
        }

    }
    void OnMouseExit()
    {
        bountyInteraction.SetActive(false);
    }
}
