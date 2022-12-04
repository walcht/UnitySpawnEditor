using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Spawn Database", menuName = "SO/Spawn Database")]
public class SpawnDatabase : ScriptableObject
{
    public List<Vector3> spawnPositions = new List<Vector3>();
}
