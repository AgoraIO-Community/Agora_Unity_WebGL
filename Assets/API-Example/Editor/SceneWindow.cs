using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Agora_RTC_Plugin.API_Example.Editor
{
    public class SceneEditorWindow : EditorWindow
    {
        List<SceneAsset> m_SceneAssets = new List<SceneAsset>();

        // Add menu item named "Example Window" to the Window menu
        [MenuItem("Agora/Build/List Scenes", false, 1)]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(SceneEditorWindow));
        }

        void OnGUI()
        {
            GUILayout.Space(8);
            if (GUILayout.Button("Search and Add Scene files"))
            {
                List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
                foreach (var file in System.IO.Directory.EnumerateFiles(".", "*.unity", System.IO.SearchOption.AllDirectories))
                {
                    string scenePath = file.Remove(0, 2);
                    UnityEngine.Debug.Log(scenePath);
                    if (!scenePath.Contains("FunctionalTest"))
                    {
                        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                    }
                }
                EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
            }

            GUILayout.Space(8);
            if (GUILayout.Button("Clear Scene files"))
            {
                EditorBuildSettings.scenes = null;
            }
        }


    }
}