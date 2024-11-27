using System.Collections.Generic;
using UnityEngine;

public class RandomSelector : MonoBehaviour
{
    public List<GameObject> objects;
    public bool chooseRandomFromStart;

    private void Start()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(false);
        }
        int R = Random.Range(0, objects.Count);
        objects[R].SetActive(true);
    }
}
