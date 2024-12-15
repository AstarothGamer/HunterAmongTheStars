using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public Transform player; 

    public Transform destination; 


    public float targetDistance = 50f; 


    void Update()
    {
        

        float distance = Vector3.Distance(player.position, destination.position);


        if (distance <= targetDistance)
        {
           

            SceneManager.LoadScene("MainGame"); 


        }
    }
}
