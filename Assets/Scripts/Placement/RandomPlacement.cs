using System.Collections.Generic;
using UnityEngine;

namespace UBUSetasVR.Placement
{
    public class RandomPlacement : BasePlacement
    {
        public GameObject[] posibleMushrooms;
        public float mushroomRadius = 5f;

        protected override void OnDrawGizmosSelected()
        {
            if (drawDebugRadius)
            {
                Transform t = GetComponent<Transform>();
                AuxiliarFunctions.DrawCircleGizmo(t, treeRadius, mushroomRadius);
            }
        }

        public override void RepeatAllPlacement()
        {
            RepeatTreesPlacement();
            RepeatMushroomsPlacement();
        }

        public void RepeatTreesPlacement()
        {
            Clear(instantiatedTrees);
            TreePlacement();
        }

        public void RepeatMushroomsPlacement()
        {
            Clear(instantiatedMushrooms);
            MushroomPlacement();
        }

        public void Clear(List<GameObject> list)
        {
            ClearList(list);
        }

        public void TreePlacement()
        {
            for (int i = 0; i < numberOfTrees; i++)
            {
                SpawnTree(treeContainer);
            }
        }

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
