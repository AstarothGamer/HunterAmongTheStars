using System.Collections.Generic;
using UnityEngine;

public class RandomSelector : MonoBehaviour
{
    public List<OBJ> objects;
    public bool chooseRandomFromStart;

    private void Start()
    {
        if (chooseRandomFromStart)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i]._gameObject.SetActive(false);
            }
            int R = Random.Range(0, objects.Count);
            objects[R]._gameObject.SetActive(true);

            if (objects[R].skybox)
            {
                RenderSettings.skybox = objects[R].skybox;
                DynamicGI.UpdateEnvironment();
            }
        }
    }
}
[System.Serializable]
public struct OBJ
{
    public GameObject _gameObject;
    public Material skybox;
}
