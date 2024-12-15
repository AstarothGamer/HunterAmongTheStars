using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData", order = 1)]
public class GameDataSO : ScriptableObject
{
    public Vector3 playerPosition;
    public int money;
    public List<string> weapons = new List<string>();

    public void ResetData()
    {
        playerPosition = Vector3.zero;
        money = 0;
        weapons = new List<string>();
    }
}
