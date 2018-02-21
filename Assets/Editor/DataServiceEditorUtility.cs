using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataService))]
public class DataServiceEditorUtility : Editor
{
    string text = "";
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DataService dataService = (DataService)target;
        text = EditorGUILayout.TextField(label:"Mushroom Name", text:text);
        if (GUILayout.Button("Read Mushroom"))
        {
            dataService.GetMushroomByName(text);
        }
    }
}
