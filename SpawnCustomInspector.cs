using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
///     Custom Inspector for SpawnDatabase Scriptable Object.
///     This custom editor helps you manage spawn positions; adding new spawns, moving existing spawns, etc...
/// </summary>
[CustomEditor(typeof(SpawnDatabase))]
public class SpawnCustomInspector : Editor
{
    SerializedProperty spawnPositions;
    SpawnDatabase editorTargetInstance;

    void OnEnable()
    {
        editorTargetInstance = (target as SpawnDatabase);
        spawnPositions = serializedObject.FindProperty("spawnPositions");

        SceneView.duringSceneGui += OnSceneGUI;                                 // weirdly OnSceneGUI did not work
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();                                              // fetch target's properties from memory
                                                                                // target's properties could've been modified elsewhere without using this instance of SerializedObject

        EditorGUILayout.PropertyField(spawnPositions, true);
         
        serializedObject.ApplyModifiedProperties();                             // not calling this will NOT update the actual value of target's properties
    }

    void OnSceneGUI(SceneView sceneView)
    {
        EditorGUI.BeginChangeCheck();

        Handles.color = Color.green;

        List<Vector3> sceneSpawnPositions = new List<Vector3>();
        for (int i = 0; i < editorTargetInstance.spawnPositions.Count; i++)
        {
            sceneSpawnPositions.Add(Handles.PositionHandle(editorTargetInstance.spawnPositions[i], Quaternion.identity));
            Handles.DrawWireCube(editorTargetInstance.spawnPositions[i], Vector3.one);
        }
        
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.Update();
            for (int i = 0; i < editorTargetInstance.spawnPositions.Count; i++)
            {
                if (spawnPositions.GetArrayElementAtIndex(i).vector3Value != sceneSpawnPositions[i]) 
                    spawnPositions.GetArrayElementAtIndex(i).vector3Value = sceneSpawnPositions[i];
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
