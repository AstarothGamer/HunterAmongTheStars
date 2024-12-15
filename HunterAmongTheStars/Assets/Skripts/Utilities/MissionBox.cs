using System.Collections;
using UnityEngine;

public class MissionBox : MonoBehaviour
{
    [SerializeField] GameObject blink;
    [SerializeField] float duration = 1f;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Victory());
        }
    }

    private IEnumerator Victory()
    {
        Time.timeScale = 0.4f;
        if (blink)
            blink.SetActive(true);

        yield return new WaitForSeconds(duration);

        SceneLoader.Instance.LoadScene("Bar");
        AudioManager.StopMusicGradually(0.8f);

        yield return new WaitForSeconds(0.9f);

        Time.timeScale = 1f;
    }
}
