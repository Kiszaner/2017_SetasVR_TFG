using System.Collections.Generic;
using UnityEngine;

namespace UBUSetasVR.Placement
{
    /// <summary>
    /// Class that implements a random placement system.
    /// </summary>
    public class RandomPlacement : BasePlacement
    {
        /// <summary>
        /// Posible mushrooms to place.
        /// </summary>
        public GameObject[] posibleMushrooms;

        /// <summary>
        /// Radius to place mushrooms.
        /// </summary>
        public float mushroomRadius = 5f;

        /// <summary>
        /// Unity method that draws Gizmo elements on the scene editor.
        /// </summary>
        protected override void OnDrawGizmosSelected()
        {
            if (drawDebugRadius)
            {
                Transform t = GetComponent<Transform>();
                AuxiliarFunctions.DrawCircleGizmo(t, treeRadius, mushroomRadius);
            }
        }

        /// <summary>
        /// Repeats the placement of both trees and mushrooms.
        /// </summary>
        public override void RepeatAllPlacement()
        {
            RepeatTreesPlacement();
            RepeatMushroomsPlacement();
        }

        /// <summary>
        /// Clears the list of instantiated trees and repeats the placement of trees.
        /// </summary>
        public void RepeatTreesPlacement()
        {
            Clear(instantiatedTrees);
            TreePlacement();
        }

        /// <summary>
        /// Clears the list of instantiated mushrooms and repeats the placement of mushrooms.
        /// </summary>
        public void RepeatMushroomsPlacement()
        {
            Clear(instantiatedMushrooms);
            MushroomPlacement();
        }

        /// <summary>
        /// Clears a list of GameObjects passed.
        /// </summary>
        /// <param name="list">List to clear</param>
        public void Clear(List<GameObject> list)
        {
            ClearList(list);
        }

        /// <summary>
        /// Places the trees.
        /// </summary>
        public void TreePlacement()
        {
            for (int i = 0; i < numberOfTrees; i++)
            {
                SpawnTree(treeContainer);
            }
        }

        /// <summary>
        /// Places the mushrooms.
        /// </summary>
        public void MushroomPlacement()
        {
            Vector3 pos;
            GameObject go;
            for (int i = 0; i < maxNumMushroomsSpawned; i++)
            {
                go = posibleMushrooms[Random.Range(0, posibleMushrooms.Length)];
                pos = Random.insideUnitCircle * mushroomRadius;
                pos = new Vector3(pos.x, 0f, pos.y);
                SpawnMushroom(go, pos);
            }
        }
    }
}
