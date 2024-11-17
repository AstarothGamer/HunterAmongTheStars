using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlasmaShooting : MonoBehaviour
{


    public GameObject plasmPref;

    public Transform spawnPoint1;

    public Transform spawnPoint2;

    public float time = 1f;

    public float speed = 1f;




    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            SpawnPlasmaObjects();


        }

        void SpawnPlasmaObjects()
        {

            Vector3 forwardDirection = transform.forward;


            GameObject obj1 = Instantiate(plasmPref, spawnPoint1.position, Quaternion.identity);


            GameObject obj2 = Instantiate(plasmPref, spawnPoint2.position, Quaternion.identity);


            StartCoroutine(MoveObject(obj1, forwardDirection));

            StartCoroutine(MoveObject(obj2, forwardDirection));

        }


        System.Collections.IEnumerator MoveObject(GameObject obj, Vector3 direction)
        {

            float elapsedTime = 0f;


            while (elapsedTime < time)           //continuation of living and moving until
            {
                obj.transform.position += direction * speed * Time.deltaTime;

                elapsedTime += Time.deltaTime;

                yield return null;  // stop corutin per frame


            }


            Destroy(obj);


        }


    }


}
