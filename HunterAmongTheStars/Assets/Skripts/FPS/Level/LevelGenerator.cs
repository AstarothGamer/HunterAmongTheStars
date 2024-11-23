using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] segmentsPrefabs;
    public float SegmentsSpacing = 50f;

    private Vector3 nextRoomPosition = Vector3.zero;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        int R = Random.Range(2, segmentsPrefabs.Length);

        for (int i = 0; i < R; i++)
        {
            GameObject randomRoom = segmentsPrefabs[Random.Range(0, segmentsPrefabs.Length)];
            Instantiate(randomRoom, nextRoomPosition, Quaternion.identity);
            nextRoomPosition += Vector3.forward * SegmentsSpacing; // Adjust for 3D placement
        }
    }
}