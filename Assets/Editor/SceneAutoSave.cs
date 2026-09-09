using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class SceneAutoSave
{
    private const double SaveInterval = 5.0;
    private static double nextSaveTime;

    static SceneAutoSave()
    {
        nextSaveTime =
            EditorApplication.timeSinceStartup + SaveInterval;

        EditorApplication.update += Update;
    }

    private static void Update()
    {
        if (EditorApplication.timeSinceStartup < nextSaveTime)
            return;

        nextSaveTime =
            EditorApplication.timeSinceStartup + SaveInterval;

        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        if (EditorApplication.isCompiling)
            return;

        if (EditorApplication.isUpdating)
            return;

        bool savedScene = false;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (!scene.isLoaded)
                continue;

            if (!scene.isDirty)
                continue;

            if (string.IsNullOrEmpty(scene.path))
                continue;

            EditorSceneManager.SaveScene(scene);
            savedScene = true;
        }

        if (savedScene)
        {
            AssetDatabase.SaveAssets();
            Debug.Log("Auto Save: Scene đã được lưu.");
        }
    }
}