using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCamera : MonoBehaviour
{
    public Transform mainCam;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = mainCam.rotation;
    }
}
