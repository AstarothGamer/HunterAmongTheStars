using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinePatrool : MonoBehaviour
{

    public Vector3 left;
    public Vector3 right;       //distinations

    public float speed = 1.2f;

    private Vector3 distination;





    private bool movingProcess = true;

    void Start()
    {
        distination = right;

    }


    //void Update()
    //{ 
    //    Moving();
    //}


    void Update()
    {

        if (movingProcess)
        {
            Moving();

        }


    }


    void Moving()
    {

        transform.position = Vector3.MoveTowards(transform.position, distination, speed * Time.deltaTime);

        if (transform.position == right) // change priority and target
        {
            distination = left;
        }

        else if (transform.position == left)
        {
            distination = right;


        }

    }


    public void StopMoving()
    {


        movingProcess = false;
    }




}
