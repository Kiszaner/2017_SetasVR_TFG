using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelScore))]
public class LevelScoreEditor : Editor
{
    string text = "1";
    bool toggle = false;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelScore levelScore = (LevelScore)target;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Score debug utilities", EditorStyles.boldLabel);
        text = EditorGUILayout.TextField(label: "Mushroom value", text: text);
        toggle = EditorGUILayout.Toggle("Is good mushroom?", toggle);
        if (GUILayout.Button("Mushroom in basket"))
        {
            Basket.RaiseEvent(toggle, int.Parse(text));
        }
    }
}