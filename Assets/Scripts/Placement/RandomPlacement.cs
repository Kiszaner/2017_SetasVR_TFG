using System.Collections;
using System.Collections.Generic;
using UBUSetasVR;
using UnityEngine;

public class RandomPlacement : MonoBehaviour
{
    public GameObject[] trees;
    public GameObject[] mushrooms;
    public float treePlacementRadius = 10;
    public float mushroomPlacementRadius = 5;
    public int treeNum = 12;
    public int mushroomNum = 6;
    public Transform treeContainer;
    public Transform mushroomContainer;
    public List<GameObject> instantiatedTrees;
    public List<GameObject> instantiatedMushrooms;

    private void OnEnable()
    {
        instantiatedTrees = new List<GameObject>();
        instantiatedMushrooms = new List<GameObject>();
	}

    private void OnDrawGizmosSelected()
    {
        Transform t = GetComponent<Transform>();
        AuxiliarFunctions.DrawCircleGizmo(t, treePlacementRadius);
    }

    public void RepeatAllPlacement()
    {
        RepeatTreesPlacement();
        RepeatMushroomsPlacement();
    }

    public void RepeatTreesPlacement()
    {
        ClearList(instantiatedTrees);
        TreePlacement();
    }

    public void RepeatMushroomsPlacement()
    {
        ClearList(instantiatedMushrooms);
        MushroomPlacement();
    }

    public void TreePlacement()
    {
        Vector3 pos;
        Quaternion rot;
        GameObject tmp;
        for (int i = 0; i < treeNum; i++)
        {
            GameObject go = trees[Random.Range(0, trees.Length)];
            pos = Random.insideUnitCircle * treePlacementRadius;
            rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            tmp = Instantiate(go, new Vector3(pos.x, 0f, pos.y), rot);
            tmp.transform.SetParent(treeContainer);
            instantiatedTrees.Add(tmp);
        }
    }

    public void MushroomPlacement()
    {
        Vector3 pos;
        Quaternion rot;
        GameObject tmp;
        for (int i = 0; i < mushroomNum; i++)
        {
            GameObject go = mushrooms[Random.Range(0, mushrooms.Length)];
            pos = Random.insideUnitCircle * mushroomPlacementRadius;
            rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            tmp = Instantiate(go, new Vector3(pos.x, 0f, pos.y), rot);
            tmp.transform.SetParent(mushroomContainer);
            instantiatedMushrooms.Add(tmp);
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
        if (!Application.isPlaying)
        {
            DestroyImmediate(go);
        }
        else
        {
            Destroy(go);
        }
#elif UNITY_ANDROID
                Destroy(go);
#endif
    }
}
