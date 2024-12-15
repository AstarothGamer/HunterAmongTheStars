using UnityEngine;

public class DynamicAudio : MonoBehaviour
{
    public Transform player; 
      

    public float minDistance = 5f; 

    public float maxDistance = 20f; 

    private AudioSource audioSource;

    void Start()
    {

        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
         
        float distance = Vector3.Distance(player.position, transform.position);

          
        if (distance <= minDistance)
        {

            audioSource.volume = 1f;  //max vol

        }
        else if (distance >= maxDistance)
        {

            audioSource.volume = 0f; 

        }
        else
        {
            
            float t = (distance - minDistance) / (maxDistance - minDistance);
             
            audioSource.volume = Mathf.Lerp(1f, 0f, t);

        }


    }

     
}
