using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeightedPlacement))]
public class WeightedPlacementEditor : Editor
{

    public override void OnInspectorGUI()
    {
        WeightedPlacement placer = (WeightedPlacement)target;

        if (GUILayout.Button("Repeat placement"))
        {
            placer.RepeatPlacement();
        }
        DrawDefaultInspector();
    }
}
