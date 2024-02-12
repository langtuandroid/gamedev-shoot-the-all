using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExampleWindow : EditorWindow
{
    List<SceneAsset> m_SceneAssets = new List<SceneAsset>();

    // Add menu item named "Example Window" to the Window menu
    [MenuItem("Window/Example Window")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(ExampleWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Scenes to include in build:", EditorStyles.boldLabel);
        for (int i = 0; i < m_SceneAssets.Count; ++i)
        {
            m_SceneAssets[i] = (SceneAsset)EditorGUILayout.ObjectField(m_SceneAssets[i], typeof(SceneAsset), false);
        }
        if (GUILayout.Button("Add"))
        {
            m_SceneAssets.Add(null);
        }
        if (GUILayout.Button("Remove Last"))
        {
            m_SceneAssets.RemoveAt(m_SceneAssets.Count - 1);
        }

        GUILayout.Space(8);

        if (GUILayout.Button("Apply To Build Settings"))
        {
            SetEditorBuildSettingsScenes();
        }
    }

    public void SetEditorBuildSettingsScenes()
    {
        // Find valid Scene paths and make a list of EditorBuildSettingsScene
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        var tempArray = new List<SceneAsset>();
        foreach (var scene in m_SceneAssets)
        {
            tempArray.Add(scene);
        }
        foreach (var scene in EditorBuildSettings.scenes)
        {
            editorBuildSettingsScenes.Add(scene);
        }
        
        int count = tempArray.Count;
        for (int i = 0; i < 10; i++)
        {
            int index = Random.Range(0, tempArray.Count);
            var sceneAsset = tempArray[index];
            string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
            if (!string.IsNullOrEmpty(scenePath))
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
            tempArray.RemoveAt(index);
        }
        // foreach (var sceneAsset in m_SceneAssets)
        // {
        //     string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
        //     if (!string.IsNullOrEmpty(scenePath))
        //         editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
        // }

        // Set the Build Settings window Scene list
        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
    }
}