using UnityEngine;

public class ContolsHint : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        gameObject.SetActive(false);
    }
}
