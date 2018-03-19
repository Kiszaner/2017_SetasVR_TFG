using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MushroomScriptableObject))]
public class MushroomScriptableObjectUtility : Editor
{
    List<Texture2D> textures;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MushroomScriptableObject mushScrObj = (MushroomScriptableObject)target;
        if (GUILayout.Button("Random pick photos"))
        {
            RandomPickPhotos(mushScrObj);
        }
        if (GUILayout.Button("Asset Path"))
        {
            string sDataPath = Application.dataPath;
            string sFolderPath = sDataPath.Substring(0, sDataPath.Length - 6) + "Assets/Art/2D/Mushrooms/Agaricus urinascens";
            Debug.Log("A: " + sFolderPath);
            string[] aFilePaths = Directory.GetFiles(sFolderPath, "*.jpg");
            textures = new List<Texture2D>();
            foreach (string sFilePath in aFilePaths)
            {
                string sAssetPath = sFilePath.Substring(sDataPath.Length - 6);
                Debug.Log(sAssetPath);

                Object objAsset = AssetDatabase.LoadAssetAtPath(sAssetPath, typeof(Object));

                textures.Add((Texture2D)objAsset);

                Debug.Log(objAsset.GetType().Name);
            }
        }
    }

    public void RandomPickPhotos(MushroomScriptableObject mushScrObj)
    {
        Texture2D[] texturesArray = textures.ToArray();
        Sprite[] sprites = new Sprite[texturesArray.Length];
        for (int i = 0; i < texturesArray.Length; i++)
        {
            sprites[i] = CreateSpriteFromTexture2D(texturesArray[i]);
        }
        mushScrObj.Photos = sprites;
    }

    private Sprite CreateSpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
