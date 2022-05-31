using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create LevelSO", fileName = "LevelSO", order = 0)]
public class LevelSO : ScriptableObject
{
    public int LevelBuildID;
    public List<Vector3> SpawnPoints;
    public int MaxCustomersSpawned;
    public bool UseOxygen;
}
