using System.Collections;
using UnityEngine;
public class KillAlll : Singleton<KillAlll>
{
    [SerializeField] GameObject blink;
    [SerializeField] float duration = 1f;

    [HideInInspector] public int enemies = 0;

    private void Start()
    {
        enemies = 0;
    }
    public void ImDead()
    {
        enemies--;
        if (enemies <= 0)
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

        SceneLoader.Instance.LoadScene("MainGame");

        yield return new WaitForSeconds(0.9f);

        Time.timeScale = 1f;
    }
}
