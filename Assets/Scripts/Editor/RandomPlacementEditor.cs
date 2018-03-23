using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomPlacement))]
public class RandomPlacementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RandomPlacement placer = (RandomPlacement)target;
        if (GUILayout.Button("Random place Trees"))
        {
            placer.TreePlacement();
        }
        if (GUILayout.Button("Random place Mushrooms"))
        {
            placer.MushroomPlacement();
        }
        if (GUILayout.Button("RemoveTrees"))
        {
            placer.ClearList(placer.InstantiatedTrees);
        }
        if (GUILayout.Button("RemoveMushrooms"))
        {
            placer.ClearList(placer.InstantiatedMushrooms);
        }
    }
}
