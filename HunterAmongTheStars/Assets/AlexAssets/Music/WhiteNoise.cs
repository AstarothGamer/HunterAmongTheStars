using UnityEngine;

public class DynamicAudio : MonoBehaviour
{
    public Transform player; // Ссылка на игрока
    public float minDistance = 5f; // Минимальное расстояние
    public float maxDistance = 20f; // Максимальное расстояние
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Вычисляем расстояние до игрока
        float distance = Vector3.Distance(player.position, transform.position);

        // Рассчитываем громкость на основе расстояния
        if (distance <= minDistance)
        {
            audioSource.volume = 1f; // Максимальная громкость
        }
        else if (distance >= maxDistance)
        {
            audioSource.volume = 0f; // Звук не слышен
        }
        else
        {
            // Пропорциональная громкость между minDistance и maxDistance
            float t = (distance - minDistance) / (maxDistance - minDistance);
            audioSource.volume = Mathf.Lerp(1f, 0f, t);
        }
    }
}
