using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnDatabase))]
public class SpawnCustomInspector : Editor
{
    SerializedProperty spawnPositions;
    SpawnDatabase editorTargetInstance;

    List<Vector3> sceneSpawnPositions;                                          // list for storing spawn position in the scene
                                                                                // declared here for optimization reasons
    private void OnEnable()
    {
        editorTargetInstance = (target as SpawnDatabase);
        spawnPositions = serializedObject.FindProperty("spawnPositions");

        SceneView.duringSceneGui += OnSceneGUI;                                 // weirdly OnSceneGUI did not work 
    }

    private void OnDisable()
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

    private void OnSceneGUI(SceneView sceneView)
    {
        EditorGUI.BeginChangeCheck();

        serializedObject.Update();

        List<Vector3> sceneSpawnPositions = new List<Vector3>();
        for (int i = 0; i < spawnPositions.arraySize; i++)
        {
            sceneSpawnPositions.Add(Handles.PositionHandle(spawnPositions.GetArrayElementAtIndex(i).vector3Value, Quaternion.identity));
        }
        
        if (EditorGUI.EndChangeCheck())
        {
            for (int i = 0; i < spawnPositions.arraySize; i++)
            {
                spawnPositions.DeleteArrayElementAtIndex(i);
                spawnPositions.InsertArrayElementAtIndex(i);
                spawnPositions.GetArrayElementAtIndex(i).vector3Value = sceneSpawnPositions[i];
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
