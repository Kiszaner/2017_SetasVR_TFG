using System.Collections.Generic;
using UnityEngine;

namespace UBUSetasVR.Placement
{
    /// <summary>
    /// Class that defines the base methods to place environmental elements in a level (mushrooms and trees). It places the elements in a circular manner based on a radius.
    /// </summary>
    public abstract class BasePlacement : MonoBehaviour
    {
        /// <summary>
        /// All posible trees to choose from and place.
        /// </summary>
        [Header("General placement settings")]
        [Tooltip("All posible trees to choose from and place")]
        public GameObject[] posibleTrees;

        /// <summary>
        /// Hierarchy element to keep placed trees organized.
        /// </summary>
        [Tooltip("Hierarchy element to keep placed trees organized")]
        public Transform treeContainer;

        /// <summary>
        /// Hierarchy element to keep mushrooms organized.
        /// </summary>
        [Tooltip("Hierarchy element to keep placed mushrooms organized")]
        public Transform mushroomContainer;

        /// <summary>
        /// Number of trees to place.
        /// </summary>
        [Tooltip("Number of trees to place")]
        public int numberOfTrees = 1500;

        /// <summary>
        /// Radius of the placement.
        /// </summary>
        [Tooltip("Radius of the placement")]
        public float treeRadius = 137f;

        /// <summary>
        /// Is visual debug radius enabled?
        /// </summary>
        [Tooltip("Is visual debug radius enabled?")]
        public bool drawDebugRadius = true;

        /// <summary>
        /// Maximum number of mushrooms to place.
        /// </summary>
        [Tooltip("Maximum number of mushrooms to place")]
        public int maxNumMushroomsSpawned = 100;

        /// <summary>
        /// List of placed trees.
        /// </summary>
        [Tooltip("List of placed trees")]
        public List<GameObject> instantiatedTrees;

        /// <summary>
        /// List of placed mushrooms.
        /// </summary>
        [Tooltip("List of placed mushrooms")]
        public List<GameObject> instantiatedMushrooms;

        /// <summary>
        /// Unity method that runs everytime the GameObject is enabled in the inspector.
        /// </summary>
        protected virtual void OnEnable()
        {
            instantiatedTrees = new List<GameObject>();
            instantiatedMushrooms = new List<GameObject>();
        }

        /// <summary>
        /// Unity method that draws Gizmo elements on the scene editor.
        /// </summary>
        protected virtual void OnDrawGizmosSelected()
        {
            if (drawDebugRadius)
            {
                Transform t = GetComponent<Transform>();
                AuxiliarFunctions.DrawCircleGizmo(t, treeRadius);
            }
        }

        /// <summary>
        /// Repeats the placement of both trees and mushrooms.
        /// </summary>
        public abstract void RepeatAllPlacement();

        /// <summary>
        /// Instantiates and places a mushroom on a certain position. Also saves the mushroom in the list of instantiated mushrooms.
        /// </summary>
        /// <param name="mushroomPrefab">Prefab of the mushroom to instantiate</param>
        /// <param name="spawnPoint">Position to place the instantiated mushroom</param>
        /// <returns>The mushroom instantiated</returns>
        protected GameObject SpawnMushroom(GameObject mushroomPrefab, Vector3 spawnPoint)
        {
            GameObject mushroom;
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            mushroom = Instantiate(mushroomPrefab, spawnPoint, rotation);
            mushroom.transform.SetParent(mushroomContainer);
            instantiatedMushrooms.Add(mushroom);
            return mushroom;
        }

        /// <summary>
        /// Instantiates and places a random tree from posible trees on a random position inside placement radius. 
        /// </summary>
        /// <param name="hierarchyGroup">Hierarchy element to make the instantiated tree child of it</param>
        /// <returns></returns>
        protected GameObject SpawnTree(Transform hierarchyGroup)
        {
            Vector3 pos;
            Quaternion rot;
            GameObject tmp;
            RaycastHit hit;
            GameObject go = posibleTrees[Random.Range(0, posibleTrees.Length)];
            pos = Random.insideUnitCircle * treeRadius;
            pos.Set(pos.x + transform.position.x, 30f, pos.y + transform.position.z);
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

        /// <summary>
        /// Clear lists of instantiated elements.
        /// </summary>
        public virtual void ClearLists()
        {
            ClearList(instantiatedTrees);
            ClearList(instantiatedMushrooms);
        }

        /// <summary>
        /// Clear the list of GameObjects passed and destroy its elements.
        /// </summary>
        /// <param name="list">The list of GameObjects to clear</param>
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

        /// <summary>
        /// Destroys the passed GameObject.
        /// </summary>
        /// <param name="go">The GameObject to destroy</param>
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
