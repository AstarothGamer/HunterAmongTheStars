using UnityEngine;

[CreateAssetMenu(fileName = "New MiniGame", menuName = "ScriptableObjects/MiniGame")]
public class MiniGame : ScriptableObject
{
    [Header("Description")]
    [TextArea(2, 10)]
    public string Name;
    [TextArea(5, 10)]
    public string Description;

    [Header("Scene to load")]
    public string Scene;
}
