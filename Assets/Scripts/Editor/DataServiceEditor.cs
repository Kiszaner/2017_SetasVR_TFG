using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UBUSetasVR.Database;
using UBUSetasVR.Database.DataStructures;
using UBUSetasVR.ScriptableObjects;

namespace UBUSetasVR.EditorScripts
{
    [CustomEditor(typeof(DataService))]
    public class DataServiceEditor : Editor
    {
        string text = "";
        MushroomList list;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DataService dataService = (DataService)target;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("DB Utilities", EditorStyles.boldLabel);
            text = EditorGUILayout.TextField(label: "Mushroom Name", text: text);
            if (GUILayout.Button("Read Mushroom from DB"))
            {
                dataService.GetMushroomByName(text);
            }
            if (!string.IsNullOrEmpty(text) && GUILayout.Button("Create Mushroom ScriptableObject"))
            {
                MushroomScriptableObject scriptableObj = CreateMushroomScriptableObject(dataService);
                if (scriptableObj != null)
                {
                    SaveScriptableObject(scriptableObj.Name, scriptableObj);
                }
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Mushroom Data assets generator", EditorStyles.boldLabel);
            if (GUILayout.Button("Create Mushrooms List"))
            {
                // TO IMPROVE
                //MushroomScriptableObject scriptableObj = CreateMushroomScriptableObject(dataService);
                //if (scriptableObj != null)
                //{
                //    MushroomList list = CreateMushroomList();
                //    list.mushroomList.Add(scriptableObj);
                //    SaveScriptableObject(scriptableObj.Name, scriptableObj);
                //}
            }
        }

        private MushroomList CreateMushroomList()
        {
            if (list == null)
            {
                list = CreateInstance<MushroomList>();
                AssetDatabase.CreateAsset(list, "Assets/Data/Mushrooms/MushroomList.asset");
                list.mushroomList = new List<MushroomScriptableObject>();
            }
            return list;
        }

        private MushroomScriptableObject CreateMushroomScriptableObject(DataService dataService)
        {
            Mushroom mushroom = dataService.GetMushroomByName(text);
            if (mushroom != null)
            {
                MushroomScriptableObject asset = CreateInstance<MushroomScriptableObject>();
                asset.DataToScriptable(mushroom);
                AssetDatabase.CreateAsset(asset, "Assets/Data/Mushrooms/" + mushroom.name + ".asset");
                return asset;
            }

            return null;
        }

        private void SaveScriptableObject(string objectName, ScriptableObject asset)
        {
            AssetDatabase.SaveAssets();
        }
    }
}