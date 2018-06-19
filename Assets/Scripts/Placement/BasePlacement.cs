using System.Collections.Generic;
using UnityEngine;

namespace UBUSetasVR.Placement
{
    public abstract class BasePlacement : MonoBehaviour
    {
        [Header("General placement settings")]
        public GameObject[] posibleTrees;
        public Transform treeContainer;
        public Transform mushroomContainer;

        public int numberOfTrees = 1500;
        public float treeRadius = 137f;
        public bool drawDebugRadius = true;

        public int maxNumMushroomsSpawned = 100;

        public List<GameObject> instantiatedTrees;
        public List<GameObject> instantiatedMushrooms;

        protected virtual void OnEnable()
        {
            instantiatedTrees = new List<GameObject>();
            instantiatedMushrooms = new List<GameObject>();
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (drawDebugRadius)
            {
                Transform t = GetComponent<Transform>();
                AuxiliarFunctions.DrawCircleGizmo(t, treeRadius);
            }
        }

        public abstract void RepeatAllPlacement();

        protected GameObject SpawnMushroom(GameObject mushroomPrefab, Vector3 spawnPoint)
        {
            GameObject mushroom;
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            mushroom = Instantiate(mushroomPrefab, spawnPoint, rotation);
            mushroom.transform.SetParent(mushroomContainer);
            instantiatedMushrooms.Add(mushroom);
            return mushroom;
        }

        protected GameObject SpawnTree(Transform hierarchyGroup)
        {
            Vector3 pos;
            Quaternion rot;
            GameObject tmp;
            RaycastHit hit;
            GameObject go = posibleTrees[Random.Range(0, posibleTrees.Length)];
            pos = Random.insideUnitCircle * treeRadius;
            pos.Set(pos.x, 30f, pos.y);
            rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            if (AuxiliarFunctions.RaycastDownToFloor(pos, out hit))
            {
                tmp = Instantiate(go, hit.point, rot);
            }
            else
            {
                Debug.LogError("NoTreeHit. Tree pos below terrain. Assigning new pos with y=0. Further errors expected");
                tmp = Instantiate(go, new Vector3(pos.x, 0f, pos.y), rot);
            }
            tmp.transform.SetParent(hierarchyGroup);
            instantiatedTrees.Add(tmp);
            return tmp;
        }

        public virtual void ClearLists()
        {
            ClearList(instantiatedTrees);
            ClearList(instantiatedMushrooms);
        }

        protected void ClearList(List<GameObject> list)
        {
            if (list != null)
            {
                foreach (GameObject go in list)
                {
                    DestroyGameObject(go);
                }
                list.Clear();
            }
        }

        public void DestroyGameObject(GameObject go)
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
}
