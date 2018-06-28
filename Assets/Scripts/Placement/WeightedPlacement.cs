using System.Collections.Generic;
using UnityEngine;

namespace UBUSetasVR.Placement
{
    /// <summary>
    /// Class that implements a weighted placement system based on custom probabilities for every mushroom species.
    /// </summary>
    public class WeightedPlacement : BasePlacement
    {
        /// <summary>
        /// Terrain, if any, used in the level as floor.
        /// </summary>
        [Header("Weighted placement settings")]
        [Tooltip("Terrain, if any, used in the level as floor")]
        public Terrain terrainUsed;

        /// <summary>
        /// Posible mushrooms to place with its own probabilities each.
        /// </summary>
        [Tooltip("Posible mushrooms to place with its own probabilities each")]
        public Mushroom[] posibleMushrooms;

        /// <summary>
        /// Number of trees per group in the hierarchy.
        /// </summary>
        [Tooltip("Number of trees per group in the hierarchy")]
        public int treesPerGroup = 5;

        /// <summary>
        /// Maximum number of mushrooms' species chosen for place near a tree.
        /// </summary>
        [Tooltip("Maximum number of mushrooms' species chosen for place near a tree")]
        public int maxMushroomChosen = 2;

        /// <summary>
        /// Minimum radius to place a mushroom near a tree.
        /// </summary>
        [Tooltip("Minimum radius to place a mushroom near a tree")]
        public float spawnMinRadius = 1.3f;

        /// <summary>
        /// Maximum radius to place a mushroom near a tree.
        /// </summary>
        [Tooltip("Maximum radius to place a mushroom near a tree")]
        public float spawnMaxRadius = 1.7f;

        /// <summary>
        /// Number of random points around a tree to be elegible to place any mushroom.
        /// </summary>
        [Tooltip("Number of random points around a tree to be elegible to place any mushroom")]
        public int pointsAroundTrees = 7;

        /// <summary>
        /// Maximum number of chosen points around a tree to place a mushroom.
        /// </summary>
        [Tooltip("Maximum number of chosen points around a tree to place a mushroom")]
        public int maxPointsChosen = 3;

        /// <summary>
        /// List of hierarchy tree groups.
        /// </summary>
        [Tooltip("List of hierarchy tree groups")]
        public List<GameObject> treeGroupsList;

        /// <summary>
        /// Mushrooms chosen to be placed.
        /// </summary>
        private Mushroom[] mushroomsChosen;

        /// <summary>
        /// Points around a tree chosen to place a mushroom.
        /// </summary>
        private Vector3[] spawnPointsChosen;

        /// <summary>
        /// Elegible points to place a mushroom.
        /// </summary>
        private Vector3[] spawnPoints;

        /// <summary>
        /// Unity method that runs everytime the GameObject is enabled in the inspector.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            treeGroupsList = new List<GameObject>();
        }

        /// <summary>
        /// Places a new tree group
        /// </summary>
        public void TreesPlacement()
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

        /// <summary>
        /// Places mushrooms for a group of trees.
        /// </summary>
        /// <param name="treesFromGroup">List of trees to place mushrooms around</param>
        private void SpawnMushroomsFromGroup(List<GameObject> treesFromGroup)
        {
            if (IsMushroomLimitReached()) return;
            float[] mushroomsProbabilities;
            // Choose posible mushrooms to spawn in this group of trees (random at the moment)
            mushroomsProbabilities = posibleMushrooms.GetProbabilities();
            mushroomsChosen = ChooseSet(mushroomsProbabilities, maxMushroomChosen);
            if (mushroomsChosen.Length < 1) return;

            // Run through trees from the group
            foreach (GameObject tree in treesFromGroup)
            {
                if (IsMushroomLimitReached()) return;
                //Choose posible points around to spawn
                spawnPoints = GetPointsAround(tree.transform.position, spawnMaxRadius, spawnMinRadius, pointsAroundTrees);

                spawnPointsChosen = AuxiliarFunctions.RandomPickWithoutRepetition(spawnPoints, maxPointsChosen);
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

        /// <summary>
        /// Gets a number of random points around a certain position within a min and max radius.
        /// </summary>
        /// <param name="position">Position to choose around</param>
        /// <param name="maxRadius">Maximum radius to get points around</param>
        /// <param name="minRadius">Minimum radius to get points around</param>
        /// <param name="maxPoints">Maximum number of points obtained</param>
        /// <returns>The random points obtained</returns>
        private Vector3[] GetPointsAround(Vector3 position, float maxRadius, float minRadius, int maxPoints)
        {
            Vector3[] points = new Vector3[maxPoints];
            for (int i = 0; i < maxPoints; i++)
            {
                points[i] = AuxiliarFunctions.PickRandomPosAroundPoint(position, maxRadius, terrainUsed, minRadius);
            }
            return points;
        }

        /// <summary>
        /// Checks if the maximum number of mushrooms allowed is reached.
        /// </summary>
        /// <returns>True if reached, false otherwise</returns>
        private bool IsMushroomLimitReached()
        {
            // To limit mushrooms spawned
            return maxNumMushroomsSpawned >= 0 && instantiatedMushrooms.Count >= maxNumMushroomsSpawned;
        }

        /// <summary>
        /// Clear lists of instantiated elements.
        /// </summary>
        public override void ClearLists()
        {
            base.ClearLists();
            ClearList(treeGroupsList);
        }

        /// <summary>
        /// Repeats the placement of both trees and mushrooms.
        /// </summary>
        public override void RepeatAllPlacement()
        {
            ClearLists();
            TreesPlacement();
        }

        /// <summary>
        /// Chooses a probability from an array of probabilities.
        /// </summary>
        /// <param name="probs">Array of probabilities to choose from</param>
        /// <returns>Index of the probability in the array</returns>
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
            float randomPoint = Random.value * total;
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
        /// <summary>
        /// Chooses mushrooms without repetition from all posible mushrooms using an array of probabilities for each mushroom.
        /// </summary>
        /// <param name="probabilities">Array of probabilities for each posible mushroom</param>
        /// <param name="numRequired">Number of mushroom required to choose</param>
        /// <returns>The mushrooms chosen</returns>
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
                    result[numToChoose] = posibleMushrooms[numLeft - 1];
                }
            }
            return result;
        }

        /// <summary>
        /// Class that defines the prefab and the probability of being choose of a mushroom.
        /// </summary>
        [System.Serializable]
        public class Mushroom
        {

            /// <summary>
            /// Prefab of the mushroom.
            /// </summary>
            [Tooltip("Prefab of the mushroom")]
            public GameObject prefab;

            /// <summary>
            /// Probability of the mushroom for being chosen.
            /// </summary>
            [Range(0f, 1f)]
            [Tooltip("Probability of the mushroom for being chosen")]
            public float probability;
        }
    }

    /// <summary>
    /// Extension class that allows simplified operations over arrays of mushrooms.
    /// </summary>
    public static class MushroomExtension
    {
        /// <summary>
        /// Gets probabilities of an array of mushrooms.
        /// </summary>
        /// <param name="mushrooms">Mushrooms to get probabilities from</param>
        /// <returns>Probabilities of the mushrooms</returns>
        public static float[] GetProbabilities(this WeightedPlacement.Mushroom[] mushrooms)
        {
            float[] result = new float[mushrooms.Length];
            for (int i = 0; i < mushrooms.Length; i++)
            {
                result[i] = mushrooms[i].probability;
            }
            return result;
        }

        /// <summary>
        /// Get prefabs of an array of mushrooms.
        /// </summary>
        /// <param name="mushrooms">Mushroom to get prefabs from</param>
        /// <returns>Prefabs of the mushrooms</returns>
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