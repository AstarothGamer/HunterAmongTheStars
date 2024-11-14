using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
                return null;

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    FindInstance();

                    // If instance is still null, create new GameObject
                    if (m_Instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        m_Instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return m_Instance;
            }
        }
    }

    private static void FindInstance()
    {
        if (m_Instance == null)
        {
            m_Instance = FindAnyObjectByType<T>();

            if (m_Instance != null)
            {
                DontDestroyOnLoad(m_Instance.gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }

    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }
}