using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineDestroyFromProjectile : MonoBehaviour
{


    public string tagTarget = "Plasma";

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
