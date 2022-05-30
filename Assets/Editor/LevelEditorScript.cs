using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditorScript : EditorWindow
{
    [MenuItem("Custom Editors/Level Editor")]
    private static void Init()
    {
        ((LevelEditorScript)EditorWindow.GetWindow(typeof(LevelEditorScript))).Show();
    }

    private Dictionary<int, string> LevelData = new Dictionary<int, string>
    {
        {0, "test"},
    };

    private void OnGUI()
    {

        bool myBool = false;
        
        GUILayout.Label("Level Settings", EditorStyles.boldLabel);
        foreach (var currentLevelData in LevelData)
        {
            EditorGUILayout.BeginToggleGroup("Level 0", true);
            EditorGUILayout.Toggle ("Toggle", myBool);
            EditorGUILayout.Toggle ("Toggle", myBool);
            EditorGUILayout.Toggle ("Toggle", myBool);
            EditorGUILayout.Toggle ("Toggle", myBool);
            EditorGUILayout.Toggle ("Toggle", myBool);
            EditorGUILayout.EndToggleGroup ();
        }
    }
}