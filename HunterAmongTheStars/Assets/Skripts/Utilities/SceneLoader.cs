using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject obj;

    private static SceneLoader _instance;

    #region Singleton
    public static SceneLoader Instance
    {
        get
        {
            // Check if the instance is already created
            if (_instance == null)
            {
                // Try to find an existing AudioManager in the scene
                _instance = FindAnyObjectByType<SceneLoader>();

                // If no AudioManager exists, create a new one
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("SceneLoader");
                    _instance = singletonObject.AddComponent<SceneLoader>();
                }

                // Make the AudioManager persist across scenes (optional)
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        // If the instance is already set, destroy this duplicate object
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;  // Assign this object as the instance
        }

        if (obj != null)
            obj.SetActive(true);
    }
    #endregion

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
        if (anim != null)
        anim.SetTrigger("End");
    }
}
