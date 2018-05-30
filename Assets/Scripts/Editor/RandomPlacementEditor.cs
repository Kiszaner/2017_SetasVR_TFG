using System.Collections;
using System.Collections.Generic;
using UBUSetasVR.Placement;
using UnityEditor;
using UnityEngine;

namespace UBUSetasVR.EditorScripts
{
    [CustomEditor(typeof(RandomPlacement))]
    public class RandomPlacementEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            RandomPlacement placer = (RandomPlacement)target;
            if (GUILayout.Button("Repeat Trees placement"))
            {
                placer.RepeatTreesPlacement();
            }
            if (GUILayout.Button("Repeat Mushrooms placement"))
            {
                placer.RepeatMushroomsPlacement();
            }
            if (GUILayout.Button("Repeat all placement"))
            {
                placer.RepeatAllPlacement();
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Remove Trees"))
            {
                placer.Clear(placer.instantiatedTrees);
            }
            if (GUILayout.Button("Remove Mushrooms"))
            {
                placer.Clear(placer.instantiatedMushrooms);
            }
            DrawDefaultInspector();
        }
    }
}