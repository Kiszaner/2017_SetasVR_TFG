using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeightedPlacement))]
public class WeightedPlacementEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WeightedPlacement placer = (WeightedPlacement)target;
        if (GUILayout.Button("Repeat choose"))
        {
            placer.TreeGroupPlacement();
        }
    }
}
