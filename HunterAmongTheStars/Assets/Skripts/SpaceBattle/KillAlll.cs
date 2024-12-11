using System.Collections;
using UnityEngine;
public class KillAlll : MonoBehaviour
{
    [SerializeField] GameObject blink;
    [SerializeField] float duration = 1f;

    [HideInInspector] public int enemies = 0;

    private static KillAlll _instance;

    #region Singleton
    public static KillAlll Instance
    {
        get
        {
            // Check if the instance is already created
            if (_instance == null)
            {
                // Try to find an existing AudioManager in the scene
                _instance = FindAnyObjectByType<KillAlll>();

                // If no AudioManager exists, create a new one
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("KillAlll");
                    _instance = singletonObject.AddComponent<KillAlll>();
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
    }
    #endregion
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

        SceneLoader.Instance.LoadScene("Bar");
        AudioManager.StopMusicGradually(0.8f);

        yield return new WaitForSeconds(0.9f);

        Time.timeScale = 1f;
    }
}
