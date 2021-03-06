﻿using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UBUSetasVR.ScriptableObjects;

namespace UBUSetasVR.EditorScripts
{
    [CustomEditor(typeof(MushroomScriptableObject))]
    public class MushroomScriptableObjectUtility : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MushroomScriptableObject mushScrObj = (MushroomScriptableObject)target;
            if (GUILayout.Button("Random pick photos"))
            {
                string sDataPath = Application.dataPath;
                string capitalizedMushName = AuxiliarFunctions.FirstUpper(mushScrObj.Name);
                string sFolderPath = sDataPath.Substring(0, sDataPath.Length - 6) + "Assets/Art/2D/Mushrooms/" + capitalizedMushName;
                Debug.Log("A: " + sFolderPath);
                string[] aFilePaths = Directory.GetFiles(sFolderPath, "*.jpg");
                List<Texture> textures;
                textures = new List<Texture>();
                foreach (string sFilePath in aFilePaths)
                {
                    string sAssetPath = sFilePath.Substring(sDataPath.Length - 6);
                    Object objAsset = AssetDatabase.LoadAssetAtPath(sAssetPath, typeof(Object));
                    textures.Add((Texture2D)objAsset);
                }
                RandomPickPhotos(mushScrObj, textures);
            }
        }

        public void RandomPickPhotos(MushroomScriptableObject mushScrObj, List<Texture> textures)
        {
            Texture[] texturesArray = textures.ToArray();
            mushScrObj.Photos = AuxiliarFunctions.RandomPickWithoutRepetition(texturesArray, 4);
            EditorUtility.SetDirty(mushScrObj);
            AssetDatabase.SaveAssets();
        }
    }
}