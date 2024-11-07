using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject obj;

    void Awake()
    {
        if (obj != null)
        obj.SetActive(true);
    }
    public void LoadScene(string name)
    {
        StartCoroutine(TransitionToScene(name));
    }
    IEnumerator TransitionToScene(string name)
    {
        if (anim != null)
        anim.SetTrigger("Start");
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(name);
    }
}
