using UnityEngine;
using UnityEditor;

public class MaterialDuplicator : EditorWindow
{
    [MenuItem("ConQ/Duplicate Material")]
    static void DuplicateMaterialWithTextures()
    {
        Material selectedMaterial = Selection.activeObject as Material;

        if (selectedMaterial == null)
        {
            Debug.LogWarning("Please select a material before running this script.");
            return;
        }

        // Create a duplicate of the selected material
        Material newMaterial = new Material(selectedMaterial);

        // Open the Save File Panel in the same folder as the selected material
        string path = AssetDatabase.GetAssetPath(selectedMaterial);
        string folderPath = System.IO.Path.GetDirectoryName(path);
        string newMaterialPath = EditorUtility.SaveFilePanelInProject("Save Material", newMaterial.name, "mat", "Save Material", folderPath);

        if (!string.IsNullOrEmpty(newMaterialPath))
        {
            // Save the new material to the selected path
            AssetDatabase.CreateAsset(newMaterial, newMaterialPath);
            AssetDatabase.Refresh();
        }
    }
}
