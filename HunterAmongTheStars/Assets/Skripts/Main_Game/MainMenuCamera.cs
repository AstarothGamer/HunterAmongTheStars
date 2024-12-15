using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private float speed = 1f; // Rotation speed

    void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }
}