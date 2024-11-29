using System.Collections;
using UnityEngine;

public class FinalEnemy : EnemyAI
{
    [SerializeField] GameObject blink;
    [SerializeField] float duration = 1f;
    public override void Die()
    {
        isDead = true;
        if (Remains != null)
        Instantiate(Remains, gameObject.transform.position, Quaternion.identity);
        StartCoroutine(Victory());
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
