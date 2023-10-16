using UnityEngine;
using UnityEditor;

public class CreatePrefabFromSelection : MonoBehaviour
{
    [MenuItem("ConQ/Create Prefabs from Selection")]
    static void CreatePrefabs()
    {
        string currentFolder = GetCurrentFolder();

        if (string.IsNullOrEmpty(currentFolder))
        {
            Debug.LogError("Could not get current folder. Please select a folder in the Project window.");
            return;
        }

        foreach (GameObject obj in Selection.gameObjects)
        {
            string prefabPath = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/{1}.prefab", currentFolder, obj.name));
            GameObject prefab = PrefabUtility.CreatePrefab(prefabPath, obj);
            Debug.Log(string.Format("Prefab saved at: {0}", AssetDatabase.GetAssetPath(prefab)));
        }
    }

    static string GetCurrentFolder()
    {
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (!string.IsNullOrEmpty(path) && System.IO.Directory.Exists(path))
            {
                return path;
            }
        }

        return null;
    }
}