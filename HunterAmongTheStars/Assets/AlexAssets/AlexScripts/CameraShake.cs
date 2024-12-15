using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform[] enemies; 

    public float triggerDistance = 5f; 

    public float shakeDuration = 0.5f; 

    public float shakeMagnitude = 0.1f; 

    private Vector3 originalLocalPosition;

    private float shakeTime = 0f;

    void Start()
    {

        originalLocalPosition = transform.localPosition;
         
    }


    void Update()
    {
        Transform closestEnemy = GetClosestEnemy();

        if (closestEnemy != null)
        {
            float distance = Vector3.Distance(closestEnemy.position, transform.position);

            if (distance <= triggerDistance)

            {

                shakeTime = shakeDuration;

            }

        }

        if (shakeTime > 0)
        {
            Shake();

            shakeTime -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalLocalPosition;
        }
    }

    Transform GetClosestEnemy()
    {

        Transform closest = null;

        float minDistance = Mathf.Infinity;


        foreach (Transform enemy in enemies)
        {

            float distance = Vector3.Distance(enemy.position, transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;

                closest = enemy;
            }
        }

        return closest;
    }

    void Shake()
    {

        Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;

        transform.localPosition = originalLocalPosition + randomOffset;

    }




}
