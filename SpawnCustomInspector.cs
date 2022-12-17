using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System;

[CustomEditor(typeof(SpawnDatabase))]
public class SpawnCustomInspector : Editor
{
    [SerializeField] VisualTreeAsset uxml_document;
    SpawnDatabase targetSpawnDatabase;
    SerializedProperty spawnPositions;

    ListView listView;                                                          // to keep global reference for the ListView

    const string _spawn_positions_size      = "spawn-positions-size";
    const string _list_view                 = "spawn-positions";
    const string _add_offset                = "add-offset";
    const string _add_button                = "add-button";
    const string _delete_button             = "delete-button";
    const string _select_all_button         = "select-all-button";
    const string _clear_selection_button    = "clear-selecton-button";

    readonly Vector3 _wire_cube_size = new Vector3(0.80f, 2.00f, 0.80f);
    readonly Vector3 _wire_cube_pos_offset = new Vector3(0, 1, 0);

    private void OnEnable()
    {
        targetSpawnDatabase = (SpawnDatabase)target;
        spawnPositions = serializedObject.FindProperty("spawnPositions");       // add exception hanbling here
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        EditorGUI.BeginChangeCheck();

        List<(int index, Vector3 position)> sceneSpawnPositions = new List<(int, Vector3)>();
        foreach (int index in listView.selectedIndices)
        {
            sceneSpawnPositions.Add((index, Handles.PositionHandle(targetSpawnDatabase.spawnPositions[index], Quaternion.identity)));
            Handles.color = Color.green;
            Handles.DrawWireCube(targetSpawnDatabase.spawnPositions[index] + _wire_cube_pos_offset, _wire_cube_size);
        }

        //for (int i = 0; i < targetSpawnDatabase.spawnPositions.Count; ++i)
        //{
        //    sceneSpawnPositions.Add(Handles.PositionHandle(targetSpawnDatabase.spawnPositions[i], Quaternion.identity));
        //    Handles.DrawWireCube(targetSpawnDatabase.spawnPositions[i], Vector3.one);
        //}

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.Update();                                          // make sure serializedObject is synchronized with
                                                                                // the spawn position database

            foreach (var item in sceneSpawnPositions)
            {
                if (spawnPositions.GetArrayElementAtIndex(item.index).vector3Value != item.position)
                    spawnPositions.GetArrayElementAtIndex(item.index).vector3Value = item.position;
            }

            serializedObject.ApplyModifiedProperties();                         // make sure to apply modified properties,
                                                                                // otherwise the spawn positions database won't 
                                                                                // get updated!
        }
    }

    public override VisualElement CreateInspectorGUI()
    {
        // Each editor window contains a root VisualElement object called rootVisualElement
        // you can specify the layout using C# scripting, but it's much more convenient to just use C# scripting for 
        // importing UXML and binding

        // Import UXML, and create a tree of VisualElements from it
        VisualElement root = uxml_document.Instantiate();

        // Exception handling for the list view. The list view name SHOULD be correctly provided
        try
        {
            listView = root.Q<ListView>(name = _list_view);

            Func<VisualElement> _makeItem = () => new Vector3Field("Spawn: ");
            Action<VisualElement, int> _bindItem =
                (visualElement, i) =>
                {
                    Vector3Field currentField = visualElement as Vector3Field;
                    currentField.BindProperty(spawnPositions.GetArrayElementAtIndex(i));
                    currentField.label = string.Concat(string.Format("Spawn {0}: ", i));
                };

            listView.itemsSource = targetSpawnDatabase.spawnPositions;
            listView.makeItem = _makeItem;
            listView.bindItem = _bindItem;

            // the scene view has to be repainted each time a selection change happens in the ListView
            // not doing this will result in lags (since the scene doesn't have to be updated because of an inspector event)
            listView.selectionChanged += (objects) =>
            {
                SceneView.lastActiveSceneView.Repaint();
            };

            SceneView.duringSceneGui += OnSceneGUI;                                 // OnSceneGUI did not work so I had to manually subscribe
                                                                                    // to the appropriate event
        }
        catch (NullReferenceException)
        {
            Debug.LogError(string.Format("{0} string isn't set correctly.", nameof(_list_view)));
            return new VisualElement();                                             // return empty inspector
        }

        // It is not necessary to provide other UI elements such as: ClearSelection Button
        try
        {
            Button addButton = root.Q<Button>(name = _add_button);

            addButton.RegisterCallback<ClickEvent>((ClickEvent evt) => {
                targetSpawnDatabase.spawnPositions.Add(Vector3.zero);
                evt.StopPropagation();
                listView.Rebuild();
            });
        }
        catch (NullReferenceException) { Debug.LogWarning(string.Format("{0} string isn't set correctly.", nameof(_add_button))); }

        try
        {
            Button deleteButton = root.Q<Button>(name = _delete_button);

            deleteButton.RegisterCallback<ClickEvent>((ClickEvent evt) => {
                foreach (int index in listView.selectedIndices)
                    targetSpawnDatabase.spawnPositions.RemoveAt(index);
                listView.RefreshItems();
                evt.StopPropagation();
            });
        }
        catch (NullReferenceException) { Debug.LogWarning(string.Format("{0} string isn't set correctly.", nameof(_delete_button))); }

        try
        {
            Button selectAllButton = root.Q<Button>(name = _select_all_button);

            selectAllButton.RegisterCallback<ClickEvent>((ClickEvent evt) => {
                listView.SetSelection(Enumerable.Range(0, listView.itemsSource.Count));
                evt.StopPropagation();
            });
        }
        catch (NullReferenceException) { Debug.LogWarning(string.Format("{0} string isn't set correctly.", nameof(_select_all_button))); }

        try
        {
            Button clearSelectionButton = root.Q<Button>(name = _clear_selection_button);
            clearSelectionButton.RegisterCallback<ClickEvent>((ClickEvent evt) => {
                listView.ClearSelection();
            });
        }
        catch (NullReferenceException) { Debug.LogWarning(string.Format("{0} string isn't set correctly.", nameof(_clear_selection_button))); }

        IntegerField spawnPositionsSizeField = root.Q<IntegerField>(name = _spawn_positions_size);
        // spawnPositionsSizeField.BindProperty();      Still don't know how to bind this to the array's size

        return root;
    }
}