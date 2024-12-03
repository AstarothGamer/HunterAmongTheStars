using UnityEngine;

public class BackgroundCamera : MonoBehaviour
{
    public float ratio = 1;
    public Transform mainCam;

    void Update()
    {
        transform.rotation = mainCam.rotation;
        GetComponent<Camera>().fieldOfView = mainCam.GetComponent<Camera>().fieldOfView * ratio;
    }
}
