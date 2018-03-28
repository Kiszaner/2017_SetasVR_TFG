using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlacement : MonoBehaviour
{
    public GameObject[] Trees;
    public GameObject[] Mushrooms;
    public float TreePlacementRadius = 10;
    public float MushroomPlacementRadius = 5;
    public int TreeNum = 12;
    public int MushroomNum = 6;
    public Transform TreeContainer;
    public Transform MushroomContainer;
    public List<GameObject> InstantiatedTrees;
    public List<GameObject> InstantiatedMushrooms;

    private void OnEnable()
    {
        InstantiatedTrees = new List<GameObject>();
        InstantiatedMushrooms = new List<GameObject>();
	}

    public void RepeatAllPlacement()
    {
        RepeatTreesPlacement();
        RepeatMushroomsPlacement();
    }

    public void RepeatTreesPlacement()
    {
        ClearList(InstantiatedTrees);
        TreePlacement();
    }

    public void RepeatMushroomsPlacement()
    {
        ClearList(InstantiatedMushrooms);
        MushroomPlacement();
    }

    public void TreePlacement()
    {
        Vector3 pos;
        Quaternion rot;
        GameObject tmp;
        for (int i = 0; i < TreeNum; i++)
        {
            GameObject go = Trees[Random.Range(0, Trees.Length)];
            pos = Random.insideUnitCircle * TreePlacementRadius;
            rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            tmp = Instantiate(go, new Vector3(pos.x, 0f, pos.y), rot);
            tmp.transform.SetParent(TreeContainer);
            InstantiatedTrees.Add(tmp);
        }
    }

    public void MushroomPlacement()
    {
        Vector3 pos;
        Quaternion rot;
        GameObject tmp;
        for (int i = 0; i < MushroomNum; i++)
        {
            GameObject go = Mushrooms[Random.Range(0, Mushrooms.Length)];
            pos = Random.insideUnitCircle * MushroomPlacementRadius;
            rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            tmp = Instantiate(go, new Vector3(pos.x, 0f, pos.y), rot);
            tmp.transform.SetParent(MushroomContainer);
            InstantiatedMushrooms.Add(tmp);
        }
    }

    public void ClearList(List<GameObject> list)
    {
        if(list != null)
        {
            foreach(GameObject go in list)
            {
                RemoveMushroomFromList(go);
            }
            list.Clear();
        }
    }

    public void RemoveMushroomFromList(GameObject go)
    {
#if UNITY_EDITOR
        DestroyImmediate(go);
#elif UNITY_ANDROID
                Destroy(go);
#endif
    }
}
