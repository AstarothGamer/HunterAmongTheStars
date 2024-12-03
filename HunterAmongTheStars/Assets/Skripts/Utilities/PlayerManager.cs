using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;

    private static PlayerManager _instance;

    #region Singleton
    public static PlayerManager Instance
    {
        get
        {
            // Check if the instance is already created
            if (_instance == null)
            {
                // Try to find an existing AudioManager in the scene
                _instance = FindAnyObjectByType<PlayerManager>();

                // If no AudioManager exists, create a new one
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("PlayerManager");
                    _instance = singletonObject.AddComponent<PlayerManager>();
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
}
