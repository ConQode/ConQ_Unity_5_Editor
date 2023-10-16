using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureCombiner : EditorWindow {

    Texture2D metalMap;
    Texture2D roughnessMap;

    [MenuItem("ConQ/Texture Combiner")]
    public static void ShowWindow() {
        EditorWindow.GetWindow<TextureCombiner>("Texture Combiner");
    }

    void OnGUI() {
        GUILayout.Label("Drag and Drop Textures Here:", EditorStyles.boldLabel);

        metalMap = (Texture2D)EditorGUILayout.ObjectField("Metal Map", metalMap, typeof(Texture2D), false);
        roughnessMap = (Texture2D)EditorGUILayout.ObjectField("Roughness Map", roughnessMap, typeof(Texture2D), false);

        if (GUILayout.Button("Combine Textures")) {
            if (metalMap == null || roughnessMap == null) {
                EditorUtility.DisplayDialog("Error", "Please drag and drop both Metal Map and Roughness Map textures.", "OK");
                return;
            }

            metalMap = MakeTextureReadable(metalMap);
            roughnessMap = MakeTextureReadable(roughnessMap);

            Texture2D combinedTexture = new Texture2D(metalMap.width, metalMap.height, TextureFormat.RGBA32, false);

            for (int y = 0; y < metalMap.height; y++) {
                for (int x = 0; x < metalMap.width; x++) {
                    Color metalColor = metalMap.GetPixel(x, y);
                    Color roughnessColor = roughnessMap.GetPixel(x, y);
                    float invertedRoughness = 1 - roughnessColor.r;
                    Color combinedColor = new Color(metalColor.r, metalColor.g, metalColor.b, invertedRoughness);
                    combinedTexture.SetPixel(x, y, combinedColor);
                }
            }

            combinedTexture.Apply();

            string initialPath = AssetDatabase.GetAssetPath(metalMap);
            string directory = Path.GetDirectoryName(initialPath);

            string filePath = EditorUtility.SaveFilePanel("Save Texture", directory, "CombinedTexture.png", "png");
            if (filePath.Length != 0) {
                byte[] bytes = combinedTexture.EncodeToPNG();
                File.WriteAllBytes(filePath, bytes);
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Texture Combined", "Texture successfully combined and saved at:\n" + filePath, "OK");
            }
        }
    }

    private Texture2D MakeTextureReadable(Texture2D texture)
    {
        string assetPath = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);
        if (!importer.isReadable) {
            importer.isReadable = true;
            AssetDatabase.ImportAsset(assetPath);
        }
        return texture;
    }
}
