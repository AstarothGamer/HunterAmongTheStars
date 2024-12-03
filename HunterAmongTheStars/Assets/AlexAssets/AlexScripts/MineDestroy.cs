using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineDestroy : MonoBehaviour
{


    public string tagTarget = "Player";

    public List<Collider> detectedObjs = new List<Collider>();




    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag == tagTarget)
        {

            Destroy(gameObject);


        }

    }


    void OnTriggerExit(Collider collider)
    {

    }
}
