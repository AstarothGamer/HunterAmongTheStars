using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineDestroyProjectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)

    {


        if (other.gameObject.tag == "Plasma")
        {

            Destroy(other.gameObject);

           



        }

    }

}