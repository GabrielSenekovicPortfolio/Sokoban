using UnityEngine;
using UnityEditor;
using System.IO;

public class MissingScriptsChecker : EditorWindow
{
    private const string assetsFolder = "Assets/Prefabs";  // The folder where to start searching for prefabs.

    [MenuItem("Tools/ShouTools/Check for Missing Scripts in Prefabs")]
    public static void CheckForMissingScriptsInPrefabs()
    {
        // Get all prefab files in the Assets folder (recursively).
        string[] prefabPaths = Directory.GetFiles(assetsFolder, "*.prefab", SearchOption.AllDirectories);

        foreach (var prefabPath in prefabPaths)
        {
            // Load the prefab asset
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab != null)
            {
                CheckForMissingScriptsInPrefab(prefab, prefabPath);
            }
        }

        Debug.Log("Finished checking for missing scripts in all prefabs.");
    }

    private static void CheckForMissingScriptsInPrefab(GameObject prefab, string prefabPath)
    {
        // Check for missing scripts in each GameObject within the prefab.
        var components = prefab.GetComponentsInChildren<MonoBehaviour>(true);  // Include inactive objects

        foreach (var component in components)
        {
            if (component == null)  // If the component is null, it means there's a missing script.
            {
                Debug.LogError($"Missing script found in prefab: {prefabPath} on GameObject: {GetFullPath(prefab)}");
                break;  // Once a missing script is found in this prefab, log it and move to the next prefab.
            }
        }
    }

    // Helper method to get the full path of a GameObject in the hierarchy (for logging).
    private static string GetFullPath(GameObject obj)
    {
        string path = obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = obj.name + "/" + path;
        }
        return path;
    }
}
