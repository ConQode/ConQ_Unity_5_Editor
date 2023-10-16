using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateMaterialsFromImages : EditorWindow
{
    [SerializeField] private Object[] selectedImages;
    private Vector2 scrollPosition;

    private SerializedObject serializedObject;
    private SerializedProperty selectedImagesProperty;

    [MenuItem("ConQ/Create Materials from Images")]
    static void Init()
    {
        CreateMaterialsFromImages window = (CreateMaterialsFromImages)EditorWindow.GetWindow(typeof(CreateMaterialsFromImages));
        window.Show();
    }

    void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        selectedImagesProperty = serializedObject.FindProperty("selectedImages");
    }

    void OnGUI()
    {
        GUILayout.Label("Selected Images", EditorStyles.boldLabel);
        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();
        serializedObject.Update();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        EditorGUILayout.PropertyField(selectedImagesProperty, true);
        EditorGUILayout.EndScrollView();
        serializedObject.ApplyModifiedProperties();
        GUILayout.Space(10);

        if (GUILayout.Button("Create Materials"))
        {
            if (selectedImages != null && selectedImages.Length > 0)
            {
                foreach (Object imageObject in selectedImages)
                {
                    Texture2D image = imageObject as Texture2D;
                    CreateMaterialFromImage(image);
                }

                Debug.Log("Materials created successfully!");
            }
            else
            {
                Debug.LogWarning("No images selected!");
            }
        }
    }

    void CreateMaterialFromImage(Texture2D image)
    {
        string materialName = image.name;
        string imagePath = AssetDatabase.GetAssetPath(image);
        string materialPath = Path.ChangeExtension(imagePath, "mat");

        Material material = new Material(Shader.Find("Standard"));
        material.mainTexture = image;

        AssetDatabase.CreateAsset(material, materialPath);
        AssetDatabase.Refresh();
    }
}
