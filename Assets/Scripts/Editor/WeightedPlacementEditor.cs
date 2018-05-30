using System.Collections;
using System.Collections.Generic;
using UBUSetasVR.Placement;
using UnityEditor;
using UnityEngine;

namespace UBUSetasVR.EditorScripts
{
    [CustomEditor(typeof(WeightedPlacement))]
    public class WeightedPlacementEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            WeightedPlacement placer = (WeightedPlacement)target;

            if (GUILayout.Button("Repeat placement"))
            {
                placer.RepeatAllPlacement();
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Remove all"))
            {
                placer.ClearLists();
            }
            DrawDefaultInspector();
        }
    }
}