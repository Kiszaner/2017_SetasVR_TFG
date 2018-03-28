using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomPlacement))]
public class RandomPlacementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RandomPlacement placer = (RandomPlacement)target;
        if (GUILayout.Button("Random place Trees"))
        {
            placer.TreePlacement();
        }
        if (GUILayout.Button("Random place Mushrooms"))
        {
            placer.MushroomPlacement();
        }

        GUILayout.Space(5);
        if (GUILayout.Button("Remove Trees"))
        {
            placer.ClearList(placer.InstantiatedTrees);
        }
        if (GUILayout.Button("Remove Mushrooms"))
        {
            placer.ClearList(placer.InstantiatedMushrooms);
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Repeat Trees placement"))
        {
            placer.RepeatTreesPlacement();
        }
        if (GUILayout.Button("Repeat Mushrooms placement"))
        {
            placer.RepeatMushroomsPlacement();
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Repeat all placement"))
        {
            placer.RepeatAllPlacement();
        }
        DrawDefaultInspector();
    }
}
