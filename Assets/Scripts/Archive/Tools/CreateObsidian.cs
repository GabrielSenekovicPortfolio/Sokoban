using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateObsidian : EditorWindow
{
    string outputDirectory = "";
    string url = "";
    Vector2 scrollPos;
    List<string> logLines = new List<string>();
    public string TaggedLink(string tag, string name) => "[" + tag + ":: [[" + name + "]]]";

    [MenuItem("Tools/ShouTools/Create Obsidian")]
    public static void ShowWindow()
    {
        GetWindow<CreateObsidian>("Obsidian creator");
    }
    private void OnGUI()
    {
        GUILayout.Label("Obsidian Creator", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        GUILayout.Label("Choose output folder", EditorStyles.label);
        EditorGUILayout.BeginHorizontal();
        outputDirectory = EditorGUILayout.TextField(outputDirectory);
        if (GUILayout.Button("Browse", GUILayout.MaxWidth(80)))
        {
            string selected = EditorUtility.OpenFolderPanel("Select Obsidian Vault Folder", "", "");
            if(!string.IsNullOrEmpty(selected))
            {
                outputDirectory = selected;
            }
        }
        EditorGUILayout.EndHorizontal();
        url = EditorGUILayout.TextField(url);
        EditorGUILayout.Space();
        if (GUILayout.Button("Create from scripts", GUILayout.MaxWidth(150)))
        {
            if(string.IsNullOrEmpty(outputDirectory))
            {
                EditorUtility.DisplayDialog("Error", "Please select a valid output directory.", "Fine!");
            }
            else
            {
                CreateObsidianFromScripts.Create(outputDirectory, logLines);
            }
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Create from google doc", GUILayout.MaxWidth(150)))
        {
            if (string.IsNullOrEmpty(outputDirectory))
            {
                EditorUtility.DisplayDialog("Error", "Please select a valid output directory.", "Fine!");
            }
            else
            {
                CreateObsidianFromGoogleDoc.Create(url, outputDirectory, logLines);
            }
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Create from markup", GUILayout.MaxWidth(150)))
        {
            string markupFile = EditorUtility.OpenFilePanel("Select Markup", "", "");
            if (string.IsNullOrEmpty(outputDirectory))
            {
                EditorUtility.DisplayDialog("Error", "Please select a valid output directory.", "Fine!");
            }
            else
            {
                CreateObsidianFromMarkup.Create(markupFile, outputDirectory);
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.Label("Logs:", EditorStyles.boldLabel);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(150));
        foreach(string line in logLines)
        {
            GUILayout.Label(line);
        }
        EditorGUILayout.EndScrollView();
    }
}
