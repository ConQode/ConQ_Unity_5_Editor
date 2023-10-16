using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class GenerateResolverFile : EditorWindow
{
    private int startValue = 0;
    private int categoryValue = 0;
    private string prefix = "";
    private string unity3dFilePath = "";

    [MenuItem("ConQ/Generate Resolver File")]
    static void Init()
    {
        GenerateResolverFile window = (GenerateResolverFile)EditorWindow.GetWindow(typeof(GenerateResolverFile));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Resolver List File Generator", EditorStyles.boldLabel);

        startValue = EditorGUILayout.IntField("Start Value (ID)", startValue);
        categoryValue = EditorGUILayout.IntField("Category Value", categoryValue);
        prefix = EditorGUILayout.TextField("Name Prefix", prefix);
        unity3dFilePath = EditorGUILayout.TextField("Unity3d File Path", unity3dFilePath);

        if (GUILayout.Button("Generate Resolver List"))
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length > 0)
                {
                    // Sort selected objects by name
                    Array.Sort(selectedObjects, (x, y) => x.name.CompareTo(y.name));
                
                    int currentID = startValue;
                    string filePath = EditorUtility.SaveFilePanel("Save Object List", "", "ObjectList.txt", "txt");
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        using (StreamWriter writer = new StreamWriter(filePath))
                        {
                            foreach (GameObject gameObject in selectedObjects)
                            {
                                writer.WriteLine(string.Format("<{0}><{1}><{2}><><{3}><{4}><><false><false><><false><><true>",
                                    currentID++, categoryValue, prefix + " " + gameObject.name, unity3dFilePath, gameObject.name));
                            }
                        }
                    }
                }
            else
            {
                EditorUtility.DisplayDialog("No objects selected", "Please select one or more objects to generate the resolver list.", "OK");
            }
        }
    }
}