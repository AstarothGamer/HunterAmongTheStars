using UnityEngine;

[CreateAssetMenu(fileName = "Bounty", menuName = "Scriptable Objects/Bounty")]
public class Bounty : ScriptableObject
{
    public string bountyName;
    public Sprite bountyPhoto;
    public int bountyReward;
    public GameObject prefab;
}