using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destination : MonoBehaviour
{
    public Transform point1;


    public Transform point2;

    Vector3 vector1;   //difference in positions of two objects

    public Text text1;


    public float distantion;


    void Update()
    {
        vector1 = point1.position - point2.position;     //Calculating the position difference vector
        text1.text = "Distance to target  " + Vector3.Distance(point1.position, point2.position);        //Text update
    }


    //void OnDrawGizmos()
    //{

    //    distantion = Vector3.Distance(point1.position, point2.position);


    //    Gizmos.color = Color.red;

    //    Gizmos.DrawLine(point1.position, point2.position);


    //}
}
