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

    private Dictionary<int, ItemSO> LevelData = new Dictionary<int, ItemSO>
    {
    };

    private void OnGUI()
    {
        GUILayout.Label("Level Settings", EditorStyles.boldLabel);
        foreach (var currentLevelData in LevelData)
        {
            
        }
    }
}