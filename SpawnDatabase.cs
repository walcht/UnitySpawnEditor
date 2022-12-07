using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Scriptable Object that holds spawn\waypoint positions as a list.
///     Positions can be easily modified using
/// </summary>
[CreateAssetMenu(fileName = "Spawn Database", menuName = "SO/Spawn Database")]
public class SpawnDatabase : ScriptableObject
{
    public List<Vector3> spawnPositions = new List<Vector3>();
}
