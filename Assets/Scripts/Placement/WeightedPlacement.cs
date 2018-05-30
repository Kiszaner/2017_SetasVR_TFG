using System.Collections;
using System.Collections.Generic;
using UBUSetasVR;
using UnityEngine;

namespace UBUSetasVR.Placement
{
    public class WeightedPlacement : BasePlacement
    {
        [Header("Weighted placement settings")]
        public Terrain terrainUsed;
        public Mushroom[] posibleMushrooms;

        public int treesPerGroup = 5;

        public int maxMushroomChosen = 2;
        public float spawnMinRadius = 1.3f;
        public float spawnMaxRadius = 1.7f;
        public int pointsAroundTrees = 7;
        public int maxPointsChosen = 3;

        public List<GameObject> treeGroupsList;

        private Mushroom[] mushroomsChosen;
        private Vector3[] spawnPointsChosen;
        private Vector3[] spawnPoints;

        protected override void OnEnable()
        {
            base.OnEnable();
            treeGroupsList = new List<GameObject>();
        }

        public void TreeGroupPlacement()
        {
            Transform treeGroup;
            int treesLeft = numberOfTrees;
            int treesWithoutPlace = numberOfTrees % treesPerGroup;
            List<GameObject> treesFromGroup;
            int k = 0;
            // Make as many Tree Groups as required
            for (k = 0; treesLeft > 0; k++)
            {
                // Make Tree group
                treeGroup = new GameObject("TreeGroup" + k).transform;
                treeGroup.transform.SetParent(treeContainer);
                treeGroupsList.Add(treeGroup.gameObject);
                treesFromGroup = new List<GameObject>();
                // Spawn Trees randomly
                if (treesLeft >= treesPerGroup)
                {
                    for (int j = 0; j < treesPerGroup; j++)
                    {
                        treesFromGroup.Add(SpawnTree(treeGroup));
                    }
                }
                treesLeft -= treesPerGroup;
                if (treesLeft < 0)
                {
                    for (int j = 0; j < treesWithoutPlace; j++)
                    {
                        treesFromGroup.Add(SpawnTree(treeGroup));
                    }
                }

                SpawnMushroomsFromGroup(treesFromGroup);
            }
        }

        private void SpawnMushroomsFromGroup(List<GameObject> treesFromGroup)
        {
            if (IsMushroomLimitReached()) return;
            float[] mushroomsProbabilities;
            // Choose posible mushrooms to spawn in this group of trees (random at the moment)
            mushroomsProbabilities = posibleMushrooms.GetProbabilities();
            mushroomsChosen = ChooseSet(mushroomsProbabilities, maxMushroomChosen);
            int i = 0;
            foreach (Mushroom mush in mushroomsChosen)
            {
                Debug.Log("Chosen " + i + ": " + mush.prefab.name);
                i++;
            }
            if (mushroomsChosen.Length < 1) return;

            // Run through trees from the group
            foreach (GameObject tree in treesFromGroup)
            {
                if (IsMushroomLimitReached()) return;
                //Choose posible points around to spawn
                spawnPoints = GetPointsAround(tree.transform.position, spawnMaxRadius, spawnMinRadius, pointsAroundTrees);

                spawnPointsChosen = ChooseSet(spawnPoints, maxPointsChosen);
                i = 0;
                foreach (Vector3 vec3 in spawnPointsChosen)
                {
                    //Debug.Log("Chosen " + i + ": " + vec3);
                    i++;
                }
                foreach (Vector3 vec3 in spawnPointsChosen)
                {
                    // Choose a mushroom index (or not) from chosen mushrooms to spawn in the point
                    float[] mushroomsChosenProbabilities = mushroomsChosen.GetProbabilities();
                    int index = Choose(mushroomsChosenProbabilities);
                    if (index != -1)
                    {
                        // Spawn the mushroom if one is chosen with random rotation
                        if (IsMushroomLimitReached()) return;
                        SpawnMushroom(mushroomsChosen[index].prefab, vec3);
                    }
                }
            }
        }

        private Vector3[] GetPointsAround(Vector3 position, float maxRadius, float minRadius, int maxPoints)
        {
            Vector3[] points = new Vector3[maxPoints];
            for (int i = 0; i < maxPoints; i++)
            {
                points[i] = AuxiliarFunctions.PickRandomPosAroundPoint(position, maxRadius, terrainUsed, minRadius);
            }
            return points;
        }

        private bool IsMushroomLimitReached()
        {
            // To limit mushrooms spawned
            return maxNumMushroomsSpawned >= 0 && instantiatedMushrooms.Count >= maxNumMushroomsSpawned;
        }

        public override void ClearLists()
        {
            base.ClearLists();
            ClearList(treeGroupsList);
        }

        public override void RepeatAllPlacement()
        {
            Debug.Log("RepeatAll");
            ClearLists();
            TreeGroupPlacement();
        }

        // Choosing Items with Different Probabilities
        private int Choose(float[] probs)
        {
            float total = 0;

            foreach (float elem in probs)
            {
                // Probability of being chosen: prob
                total += elem;
                // Probability of not being chosen: 1 - prob
                total += 1 - elem;
            }

            //Debug.Log("Total: " + total);

            float randomPoint = Random.value * total;

            //Debug.Log("Point: " + randomPoint);
            for (int i = 0; i < probs.Length; i++)
            {
                if (randomPoint < probs[i])
                {
                    return i;
                }
                else
                {
                    randomPoint -= probs[i];
                }
            }
            return -1;
        }

        // Sin repeticiones int
        private Mushroom[] ChooseSet(float[] probabilities, int numRequired)
        {
            Mushroom[] result = new Mushroom[numRequired];

            int numToChoose = numRequired;

            for (int numLeft = probabilities.Length; numToChoose > 0 && numLeft > 0; numLeft--)
            {
                float prob = (float)numToChoose / (float)numLeft;

                if (Random.value <= prob)
                {
                    numToChoose--;
                    result[numToChoose] = posibleMushrooms[numLeft - 1]; //probabilities[numLeft - 1];
                }
            }
            return result;
        }

        // Sin repeticiones Transform
        private Vector3[] ChooseSet(Vector3[] spawnPoints, int numRequired)
        {
            Vector3[] result = new Vector3[numRequired];

            int numToChoose = numRequired;

            for (int numLeft = spawnPoints.Length; numToChoose > 0 && numLeft > 0; numLeft--)
            {

                float prob = (float)numToChoose / (float)numLeft;

                if (Random.value <= prob)
                {
                    numToChoose--;
                    result[numToChoose] = spawnPoints[numLeft - 1];
                }
            }
            return result;
        }

        [System.Serializable]
        public class Mushroom
        {
            public GameObject prefab;
            [Range(0f, 1f)]
            public float probability;
        }
    }

    public static class MushroomExtension
    {
        public static float[] GetProbabilities(this WeightedPlacement.Mushroom[] mushrooms)
        {
            float[] result = new float[mushrooms.Length];
            for (int i = 0; i < mushrooms.Length; i++)
            {
                result[i] = mushrooms[i].probability;
            }
            return result;
        }

        public static GameObject[] GetPrefabs(this WeightedPlacement.Mushroom[] mushrooms)
        {
            GameObject[] result = new GameObject[mushrooms.Length];
            for (int i = 0; i < mushrooms.Length; i++)
            {
                result[i] = mushrooms[i].prefab;
            }
            return result;
        }
    }
}